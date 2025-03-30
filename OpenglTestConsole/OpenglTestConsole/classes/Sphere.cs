using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Sphere : Mesh
    {
        public int SectorCount;
        public int StackCount;
        public float radius;
        public uint[] indices;
        public Texture Texture;

        [SetsRequiredMembers]
        // TODO: make it so that if theres no texture, its just a color thats specified
        public Sphere(int stackCount, int sectorCount, float radius, Camera camera, Texture texture = null)
            : base(camera: camera)
        {
            if (texture == null)
                texture = new("textures/PlaceHolder.png");
            texture.Init();

            this.StackCount = stackCount;
            this.SectorCount = sectorCount;
            this.radius = radius;
            this.Texture = texture;

            this.Shader = new Shader("shaders/sphere.vert", "shaders/sphere.frag");
            this.Shader.Init();

            // what we need to do is:
            // get the shit from the GetSphere cuh
            // send them to the mesh data cuh
            // then just, nothing rly

            (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, this.indices, _) = GetSphere();

            this.size = vertices.Length; // almost forgor this lmao
            this.SetVector3(vertices, 0);
            this.SetVector3(normals, 1);
            this.SetVector2(texCoords, 2);
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
                    if (i != (StackCount - 1))
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
            this.Shader.Use();

            this.Shader.SetTexture("tex", this.Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);

            this.Shader.SetVector3("lightPos", light.Location);
            this.Shader.SetVector3("lightColor", new Vector3(light.Color));

            this.Shader.SetVector3("viewPos", this.Camera.Position);

            GL.Enable(EnableCap.CullFace);
            this.Render(this.indices, type);
            GL.Disable(EnableCap.CullFace);
        }
    }
}
