using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.api.rendering;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.Paths;
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
    public class Cylinder : LightEffectedMesh
    {
        private int StackCount;
        private int SectorCount;
        private float Height;
        private float Radius;
        private Texture Texture;
        private Vector4 Color;
        private bool UsesTexture = false;

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
                Shader = Resources.Shaders[ResourcePaths.Shaders.objectTextured];
            else
                Shader = Resources.Shaders[ResourcePaths.Shaders.objectMonoColor];

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
            for (int i = 0; i <= StackCount; i++)
            {
                float stackStep = Height / StackCount;
                float h = -Height / 2.0f + i * stackStep;
                float t = 1.0f - i;                              // vertical tex coord; 1 to 0

                for (int j = 0; j < SectorCount; j++)
                {
                    Vector3 unit = unitVertices[j];

                    vertices.Add(new(
                        unit.X * Radius,                // vx
                        unit.Y * Radius,                // vy
                        h                                   // vz
                    ));

                    // normal vector
                    normals.Add(new(
                        unit.X,                         // nx
                        unit.Y,                         // ny
                        unit.Z                          // nz
                    ));

                    // texture coordinate
                    texCoords.Add(new(
                        j / SectorCount,    // s
                        t                           // t
                    ));
                }
            }

            #region top and bottom vertices
            // bottom Center point
            float lidHeight = -(Height / 2f);
            float nz = -1;
            vertices.Add(new Vector3(0, 0, lidHeight));
            normals.Add(new Vector3(0, 0, nz));
            texCoords.Add(new Vector2(0.5f, 0.5f));

            // top center point
            lidHeight = (Height / 2f);
            nz = -1;
            vertices.Add(new Vector3(0, 0, lidHeight));
            normals.Add(new Vector3(0, 0, nz));
            texCoords.Add(new Vector2(0.5f, 0.5f));
            #endregion

            List<uint> indices = new List<uint>();
            uint uintSectorCount = Convert.ToUInt32(SectorCount);
            uint uintStackCount = Convert.ToUInt32(StackCount);

            #region side indices

            for (uint i = 0; i < StackCount; ++i)
            {
                uint k1 = i * uintSectorCount;          // beginning of current stack
                uint k2 = k1 + uintSectorCount;       // beginning of next stack

                for (int j = 0; j < SectorCount; j++, ++k1, ++k2)
                {
                    // First triangle of quad
                    indices.Add(k1);
                    indices.Add(k1 + 1);
                    indices.Add(k2);

                    // 0 2 5 
                    // Second triangle of quad
                    if (j + 1 != SectorCount)
                    {// normal stuff
                        indices.Add(k2);         // 5
                        indices.Add(k1 + 1);    // 3
                        indices.Add(k2 + 1);    // 6
                    }
                    else
                    { // we need 3 0 and 2
                        indices.Add(k1); // 2
                        indices.Add(uintSectorCount * i); // 0
                        indices.Add(k1 + 1); // 3
                        //indices.Add(Math.Min(k1 - uintSectorCount, 0));
                        //indices.Add((uintSectorCount * i));
                    }
                }
            }

            #endregion

            #region top and bottom indices

            #region top

            uint bottomIndex = (uint)(vertices.Count - 2); // bottom center point
            uint bottomK1 = 0;
            uint bottomK2 = 1;

            for (int j = 0; j < SectorCount; j++, bottomK1++, bottomK2++)
            {
                // First triangle 
                // 3 is the next stacks starting index, which is the number of sectors we have
                // so it not being 3 means we are in the same stack
                if (bottomK2 != SectorCount)
                    indices.Add(bottomK2);
                else
                    indices.Add(bottomK2 - uintSectorCount); // 0
                indices.Add(bottomK1);
                indices.Add(bottomIndex); // center point
            }

            #endregion

            #region bottom

            uint topIndex = (uint)(vertices.Count - 1);             // top center point
            uint topK1 = uintSectorCount * uintStackCount;         // beginning of current stack
            uint topK2 = topK1 + 1;                                                     // beginning of next stack

            for (int j = 0; j < SectorCount; j++, topK1++, topK2++)
            {
                // First triangle of quad
                indices.Add(topIndex); // center point
                indices.Add(topK1);
                // 3 is the next stacks starting index, which is the number of sectors we have
                // so it not being 3 means we are in the same stack
                if (topK2 != vertices.Count - 2)
                    indices.Add(topK2);
                else
                    indices.Add(uintSectorCount * uintStackCount); // 0
            }

            #endregion

            #endregion

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
            for (int i = 0; i < sectorCount; i++)
            {
                float currentAngle = i * (360f / sectorCount); // current angle in degrees

                float x = MathMisc.Cosf(currentAngle);
                float y = MathMisc.Sinf(currentAngle);

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
    }
}
