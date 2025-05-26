using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    public class Sphere : Geometry3D
    {
        public int SectorCount;
        public int StackCount;
        public float radius;

        public Sphere(int stackCount, int sectorCount, float radius)
        {
            this.StackCount = stackCount;
            this.SectorCount = sectorCount;
            this.radius = radius;
            Init();
        }

        private void Init()
        {
            (this.Vertices, this.Normals, this.TexCoords, this.Indices, _) = GetSphere();
            Transform tempTrans = new Transform();
            tempTrans.Rotation.Y = -90;
            tempTrans.UpdateMatrix();
            this.ApplyTransformation(tempTrans.GetModelMatrix());
        }

        public (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices,
            uint[] lineIndices
        ) GetSphere()
        {
            // make variables for quick access as we are using floats and all deez are returning doubles

            // list because itd be annoying to use arrays from the start
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();

            float lengthInv = 1.0f / radius;
            float sectorStep = 2 * MathMisc.PI / SectorCount;
            float stackStep = MathMisc.PI / StackCount;

            for (int i = 0; i <= StackCount; i++)
            {
                float stackAngle = MathMisc.PI / 2 - i * stackStep; // starting from pi/2 to -pi/2
                float xy = radius * MathMisc.CosfRad(stackAngle); // r * cos(u)
                float z = radius * MathMisc.SinfRad(stackAngle); // r * sin(u)

                for (int j = 0; j <= SectorCount; j++)
                {
                    float sectorAngle = j * sectorStep; // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    float x = xy * MathMisc.CosfRad(sectorAngle); // r * cos(u) * cos(v)
                    float y = xy * MathMisc.SinfRad(sectorAngle); // r * cos(u) * sin(v)
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
                uint k1 = (uint)(i * (SectorCount + 1)); // beginning of current stack
                uint k2 = (uint)(k1 + SectorCount + 1); // beginning of next stack

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
                    if (i != 0) // horizontal lines except 1st stack, k1 => k+1
                    {
                        lineIndices.Add(k1);
                        lineIndices.Add(k1 + 1);
                    }
                }
            }

            return (
                vertices: vertices.ToArray(),
                normals: normals.ToArray(),
                texCoords: texCoords.ToArray(),
                indices: indices.ToArray(),
                lineIndices: lineIndices.ToArray()
            );
        }

        public override void Apply(BufferManager BufferManager)
        {
            base.Apply(BufferManager);
        }
    }
}
