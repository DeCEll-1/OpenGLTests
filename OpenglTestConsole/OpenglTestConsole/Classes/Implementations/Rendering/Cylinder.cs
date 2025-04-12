using OpenglTestConsole.classes;
using OpenglTestConsole.classes.api.rendering;
using OpenglTestConsole.Classes.API.Misc;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    internal class Cylinder : Mesh
    {
        private int StackCount;
        private int SectorCount;
        private float Height;
        private float Radius;
        private Texture Texture;
        private Vector4 Color;
        private bool UsesTexture = false;
        private uint[] indices;

        // its not that hard to draw a FUCKİNG CYLİNDER
        // YOU NEED TO DRAW A CİRCLE
        // sin cos gives dot from angle
        // then you star from angle 0, then increase it by 360/sector count

        [SetsRequiredMembers]
        public Cylinder(Camera camera, int StackCount, int SectorCount, float Height, float Radius, Texture texture) : base(camera)
        {
            this.StackCount = StackCount; this.SectorCount = SectorCount; this.Height = Height; this.Radius = Radius; this.Texture = texture;
            UsesTexture = true;
            Init();
        }
        [SetsRequiredMembers]
        public Cylinder(Camera camera, int StackCount, int SectorCount, float Height, float Radius, Vector4 color) : base(camera)
        {
            this.StackCount = StackCount; this.SectorCount = SectorCount; this.Height = Height; this.Radius = Radius; this.Color = color;
            Init();
        }
        private void Init()
        {
            if (this.UsesTexture)
                Shader = Main.Shaders["objectTextured"];
            else
                Shader = Main.Shaders["objectMonoColor"];

            (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] indices) = GetCylinder();
            SetVector3(vertices, 0);
            SetVector3(normals, 1);
            if (this.UsesTexture)
                SetVector2(texCoords, 2);
            SetIndices(indices);
            this.indices = indices;
            //SetColor(new Color4[vertices.Length], 3);
            //SetMatrix4(new Matrix4[vertices.Length], 4);
            //Render(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles);
        }

        private (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] indices) GetCylinder()
        {
            List<Vector3> unitVertices = GetUnitVertices(SectorCount);

            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<Vector2> texCoords = new();

            // put side vertices to arrays
            for (int i = 0; i < StackCount; ++i)
            {
                float stackStep = Height / StackCount;
                float h = -Height / 2.0f + i * stackStep;
                float t = 1.0f - i;                              // vertical tex coord; 1 to 0

                for (int j = 0; j <= SectorCount; ++j)
                {
                    float ux = unitVertices[j].X;
                    float uy = unitVertices[j].Y;
                    float uz = unitVertices[j].Z;

                    vertices.Add(new(
                        ux * Radius,                // vx
                        uy * Radius,                // vy
                        h                                // vz
                    ));

                    // normal vector
                    normals.Add(new(
                        ux, // nx
                        uy, // ny
                        uz   // nz
                    ));

                    // texture coordinate
                    texCoords.Add(new(
                        j / SectorCount,    // s
                        t                           // t
                    ));
                }
            }

            // the starting index for the base/top surface
            uint baseCenterIndex = (uint)vertices.Count;
            uint topCenterIndex = (uint)(baseCenterIndex + SectorCount + 1); // include center vertex
            //uint baseCenterIndex = (uint)((StackCount + 1) * (SectorCount + 1));
            //uint topCenterIndex = (uint)(baseCenterIndex + SectorCount + 1);

            // Put base and top vertices to arrays
            for (int i = 0; i < 2; ++i)
            {
                float h = -Height / 2.0f + i * Height;      // z value; -h/2 to h/2
                float nz = -1 + i * 2;                      // z value of normal; -1 to 1

                // Center point
                vertices.Add(new Vector3(0, 0, h));
                normals.Add(new Vector3(0, 0, nz));
                texCoords.Add(new Vector2(0.5f, 0.5f));

                for (int j = 0; j < SectorCount; ++j)
                {
                    Vector3 unit = unitVertices[j];
                    // Position vector
                    vertices.Add(new Vector3(unit.X * Radius, unit.Y * Radius, h));
                    // Normal vector
                    normals.Add(new Vector3(0, 0, nz));
                    // Texture coordinate
                    texCoords.Add(new Vector2(-unit.X * 0.5f + 0.5f, -unit.Y * 0.5f + 0.5f));
                }
            }


            List<uint> indices = new List<uint>();

            uint ringVertexCount = (uint)(SectorCount + 1);

            for (uint i = 0; i < StackCount; ++i)
            {
                uint k1 = i * ringVertexCount;         // beginning of current stack ring
                uint k2 = k1 + ringVertexCount;        // beginning of next stack ring

                for (int j = 0; j < SectorCount; ++j, ++k1, ++k2)
                {
                    // First triangle of quad
                    indices.Add(k1);
                    indices.Add(k1 + 1);
                    indices.Add(k2);

                    // Second triangle of quad
                    indices.Add(k2);
                    indices.Add(k1 + 1);
                    indices.Add(k2 + 1);
                }
            }

            // indices for the base surface
            for (uint i = 0, k = baseCenterIndex + 1; i < SectorCount; ++i, ++k)
            {
                if (i < SectorCount - 1)
                {
                    indices.Add(baseCenterIndex);
                    indices.Add(k + 1);
                    indices.Add(k);
                }
                else // wrap around for the last triangle
                {
                    indices.Add(baseCenterIndex);
                    indices.Add(baseCenterIndex + 1);
                    indices.Add(k);
                }
            }

            // indices for the top surface
            for (uint i = 0, k = topCenterIndex + 1; i < SectorCount; ++i, ++k)
            {
                if (i < SectorCount - 1)
                {
                    indices.Add(topCenterIndex);
                    indices.Add(k);
                    indices.Add(k + 1);
                }
                else // wrap around for the last triangle
                {
                    indices.Add(topCenterIndex);
                    indices.Add(k);
                    indices.Add(topCenterIndex + 1);
                }
            }

            return (
                vertices.ToArray(),
                normals.ToArray(),
                texCoords.ToArray(),
                indices.ToArray()
            );

        }

        private List<Vector3> GetUnitVertices(int sectorCount)
        {
            List<Vector3> unitVertices = new();
            for (int i = 0; i <= sectorCount; i++)
            {
                float currentAngle = i * (360f / sectorCount); // current angle in degrees

                float x = MathMisc.cosf(currentAngle);
                float y = MathMisc.sinf(currentAngle);

                unitVertices.Add(new(x, y, 0));
            }

            return unitVertices;
        }

        public new void Render(OpenTK.Graphics.OpenGL4.PrimitiveType type = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles)
        {
            //Shader.Use();

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);

            //GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
            Render(this.indices, type);
            //GL.Disable(EnableCap.CullFace);
        }
        public void SetStaticUniforms(Light light)
        {
            Shader.SetVector3("lightPos", light.Location);
            Shader.SetVector4("lightColorIn", light.Color);
            Shader.SetVector4("ambientIn", light.Ambient);

            Shader.SetVector3("viewPos", Camera.Position);
        }
        public void PrepareRender(Light light)
        {
            this.Shader.Use();
            this.SetStaticUniforms(light);
            GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
        }
        public void EndRender() => GL.Disable(EnableCap.CullFace);
    }
}
