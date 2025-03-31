using OpenglTestConsole.classes.api.rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes.impl.rendering
{
    public class Sphere : Mesh
    {
        public int SectorCount;
        public int StackCount;
        public float radius;
        public uint[] indices;
        public Texture Texture;
        public Vector4 Color;
        public bool UsesTexture = false;

        // TODO: make it so that if theres no texture, its just a color thats specified
        [SetsRequiredMembers]
        public Sphere(Camera camera, int stackCount, int sectorCount, float radius, Texture texture)
            : base(camera: camera)
        {
            this.StackCount = stackCount;
            this.SectorCount = sectorCount;
            this.radius = radius;
            this.Texture = texture;
            this.UsesTexture = true;
            Init();
        }

        [SetsRequiredMembers]
        public Sphere(Camera camera, int stackCount, int sectorCount, float radius, Vector4 color)
            : base(camera: camera)
        {
            this.StackCount = stackCount;
            this.SectorCount = sectorCount;
            this.radius = radius;
            this.Color = color;
            Init();
        }

        private void Init()
        {
            if (this.UsesTexture)
                Shader = Main.Shaders["sphereTexture"];
            else
                Shader = Main.Shaders["sphereMonoColor"];

            if (Shader.initalised == false)
                Shader.Init();

            // what we need to do is:
            // get the shit from the GetSphere cuh
            // send them to the mesh data cuh
            // then just, nothing rly

            (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, indices, _) = GetSphere();

            size = vertices.Length; // almost forgor this lmao
            SetVector3(vertices, 0);
            SetVector3(normals, 1);
            if (this.UsesTexture)
                SetVector2(texCoords, 2);

            GL.BindVertexArray(VertexArrayObjectPointer);

            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] indices, uint[] lineIndices) GetSphere()
        {
            // make variables for quick access as we are using floats and all deez are returning doubles
            float PI = (float)Math.PI;
            Func<double, float> cosf = delegate (double x) { return (float)Math.Cos(x); };
            Func<double, float> sinf = delegate (double x) { return (float)Math.Sin(x); };

            // list because itd be annoying to use arrays from the start
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();

            float lengthInv = 1.0f / radius;
            float sectorStep = 2 * PI / SectorCount;
            float stackStep = PI / StackCount;

            for (int i = 0; i <= StackCount; i++)
            {
                float stackAngle = PI / 2 - i * stackStep;        // starting from pi/2 to -pi/2
                float xy = radius * cosf(stackAngle);             // r * cos(u)
                float z = radius * sinf(stackAngle);              // r * sin(u)

                for (int j = 0; j <= SectorCount; j++)
                {
                    float sectorAngle = j * sectorStep;           // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    float x = xy * cosf(sectorAngle);             // r * cos(u) * cos(v)
                    float y = xy * sinf(sectorAngle);             // r * cos(u) * sin(v)
                    vertices.Add(new Vector3(x, y, z));

                    float normalisedX = x * lengthInv;
                    float normalisedY = y * lengthInv;
                    float normalisedZ = z * lengthInv;
                    normals.Add(new Vector3(normalisedX, normalisedY, normalisedZ));

                    // vertex tex coord (s, t) range between [0, 1]
                    float s = (float)j / SectorCount;
                    float t = (float)i / StackCount;
                    texCoords.Add(new Vector2(s, t));
                }
            }


            // generate CCW index list of sphere triangles
            // k1--k1+1
            // |  / |
            // | /  |
            // k2--k2+1
            List<uint> indices = new List<uint>();
            List<uint> lineIndices = new List<uint>(); // for wireframe and blabla
            for (int i = 0; i < StackCount; ++i)
            {
                uint k1 = (uint)(i * (SectorCount + 1));     // beginning of current stack
                uint k2 = (uint)(k1 + SectorCount + 1);      // beginning of next stack

                for (int j = 0; j < SectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding first and last stacks
                    // k1 => k2 => k1+1
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if (i != StackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }

                    // store indices for lines
                    // vertical lines for all stacks, k1 => k2
                    lineIndices.Add(k1);
                    lineIndices.Add(k2);
                    if (i != 0)  // horizontal lines except 1st stack, k1 => k+1
                    {
                        lineIndices.Add(k1);
                        lineIndices.Add(k1 + 1);
                    }
                }
            }

            return
                (
                vertices: vertices.ToArray(),
                normals: normals.ToArray(),
                texCoords: texCoords.ToArray(),
                indices: indices.ToArray(),
                lineIndices: lineIndices.ToArray()
                );
            // plz work i dont know how this works
        }

        // TODO: make this take a light array or list instead of a singular light cuz, we can, yk, use more lights
        public void Render(Light light, OpenTK.Graphics.OpenGL4.PrimitiveType type = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles)
        {
            Shader.Use();

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);

            Shader.SetVector3("lightPos", light.Location);
            Shader.SetVector4("lightColorIn", light.Color);
            Shader.SetVector4("ambientIn", light.Ambient);

            Shader.SetVector3("viewPos", Camera.Position);

            GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
            Render(indices, type);
            GL.Disable(EnableCap.CullFace);
        }
    }
}
