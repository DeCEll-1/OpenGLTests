using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Sphere
    {
        public int SectorCount;
        public int StackCount;
        public float radius;
        public bool SmoothShading;

        public Sphere(int stackCount, int sectorCount, float radius, bool smoothShading = true)
        {
            this.StackCount = stackCount;
            this.SectorCount = sectorCount;
            this.radius = radius;
            this.SmoothShading = smoothShading;
        }

        public (Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, Vector3i[] indices, Vector3i[] lineIndices) GetSphere()
        {
            // make variables for quick access as we are using floats and all deez are returning doubles
            float PI = (float)Math.PI;
            Func<double, float> cosf = delegate (double x) { return (float)Math.Cos(x); };
            Func<double, float> sinf = delegate (double x) { return (float)Math.Sin(x); };

            // list because itd be annoying to use arrays from the start
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();

            List<Vector3i> lineIndices = new List<Vector3i>();
            List<Vector3i> indices = new List<Vector3i>();

            float lengthInv = 1.0f / radius;
            float sectorStep = 2 * PI / SectorCount;
            float stackStep = PI / StackCount;

            for (int i = 0; i < StackCount; i++)
            {
                float stackAngle = PI / 2 - i * stackStep;        // starting from pi/2 to -pi/2
                float xy = radius * cosf(stackAngle);             // r * cos(u)
                float z = radius * sinf(stackAngle);              // r * sin(u)

                for (int j = 0; j < SectorCount; j++)
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
            List<int> indicesInt = new List<int>();
            List<int> lineIndicesInt = new List<int>(); // for wireframe and blabla
            int k1, k2;
            for (int i = 0; i < StackCount; ++i)
            {
                k1 = i * (SectorCount + 1);     // beginning of current stack
                k2 = k1 + SectorCount + 1;      // beginning of next stack

                for (int j = 0; j < SectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding first and last stacks
                    // k1 => k2 => k1+1
                    if (i != 0)
                    {
                        indicesInt.Add(k1);
                        indicesInt.Add(k2);
                        indicesInt.Add(k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if (i != (StackCount - 1))
                    {
                        indicesInt.Add(k1 + 1);
                        indicesInt.Add(k2);
                        indicesInt.Add(k2 + 1);
                    }

                    // store indices for lines
                    // vertical lines for all stacks, k1 => k2
                    lineIndicesInt.Add(k1);
                    lineIndicesInt.Add(k2);
                    if (i != 0)  // horizontal lines except 1st stack, k1 => k+1
                    {
                        lineIndicesInt.Add(k1);
                        lineIndicesInt.Add(k1 + 1);
                    }
                }
            }

            if (lineIndices.Count % 3 != 0)
            {
                Console.WriteLine("Line Indices while generating Sphere DİED and gave .Count % 3 != 0");
            }
            if (indices.Count % 3 != 0)
            {
                Console.WriteLine("Indices while generating Sphere DİED and gave .Count % 3 != 0");
            } // pray that this never happens

            for (int i = 0; i < lineIndicesInt.Count; i += 3)
            { // turn them to vector3s cuz we are using vector 3s
                float x = lineIndicesInt[i]; // 0
                float y = lineIndicesInt[i + 1]; // 1
                float z = lineIndicesInt[i + 2]; // 2
                lineIndices.Add(new Vector3i((int)x, (int)y, (int)z));
            }

            for (int i = 0; i < indicesInt.Count; i += 3)
            { // turn them to vector3s cuz we are using vector 3s
                float x = indicesInt[i]; // 0
                float y = indicesInt[i + 1]; // 1
                float z = indicesInt[i + 2]; // 2
                indices.Add(new Vector3i((int)x, (int)y, (int)z));
            }

            return
                (
                vertices: vertices.ToArray(),
                normals: normals.ToArray(),
                texCoords: texCoords.ToArray(),
                indices: indices.ToArray(),
                lineIndices: lineIndices.ToArray()
                );

        }


    }
}
