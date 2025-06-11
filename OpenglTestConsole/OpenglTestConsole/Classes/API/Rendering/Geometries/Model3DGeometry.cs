using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    public class Model3DGeometry : Geometry3D
    {
        public string Path { get; }

        public Model3DGeometry(string path)
        {
            Path = path;
            Init();
        }

        private void Init()
        {
            (this.Vertices, this.Normals, this.TexCoords, this.Indices) = LoadFromOBJ(Path);
        }

        public override void Apply(BufferManager BufferManager)
        {
            BufferManager.SetVector3(Vertices, 0);
            BufferManager.SetVector3(Normals, 1);
            BufferManager.SetVector2(TexCoords, 2);
            BufferManager.SetIndices(Indices);
        }

        public static (Vector3[] Vertices, Vector3[] Normals, Vector2[] TexCoords, uint[] indices) LoadFromOBJ(string path)
        {
            List<Vector3> vertices = [];
            List<Vector3> finalVertices = [];
            List<Vector3> normals = [];
            List<Vector3> finalNormals = [];
            List<Vector2> texCoords = [];
            List<Vector2> finalTexCoords = [];
            List<uint> indices = [];
            Dictionary<(int, int, int), uint> uniqueMap = [];

            Logger.BeginTimingBlock();

            using var reader = new StreamReader(path);

            string? line = "";
            uint nextIndex = 0;
            while ((line = reader.ReadLine()) != null)
            {
                bool skip = true;
                string op = "";
                foreach (string c in new[] { "v ", "vt", "vn", "f " })
                    if (skip == true && line.Length > 3 && line[0] == c[0] && line[1] == c[1]) // check if we care about the line
                    {
                        op = c;
                        skip = false;
                        continue;
                    }
                if (skip)
                    continue;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (op == "v ")
                {
                    vertices.Add(new(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)
                    ));
                    continue;
                }
                if (op == "vt")
                {
                    texCoords.Add(new(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture)
                    ));
                    continue;
                }
                if (op == "vn")
                {
                    normals.Add(new(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)
                    ));
                    continue;
                }
                if (op == "f ")
                {
                    // iterate over the triangle corners
                    for (int i = 1; i <= 3; i++)
                    {
                        // split the vertex values
                        var indice = parts[i].Split('/');

                        // convert 1 based system to 0 based system
                        int vIdx = int.Parse(indice[0]) - 1;
                        int tIdx = (indice.Length > 1 && indice[1] != "") ? int.Parse(indice[1]) - 1 : -1;
                        int nIdx = (indice.Length > 2 && indice[2] != "") ? int.Parse(indice[2]) - 1 : -1;

                        // key to check for uniques
                        var key = (vIdx, tIdx, nIdx);

                        // check if we already added this vertex
                        if (!uniqueMap.TryGetValue(key, out uint index))
                        {
                            // create the new vertex
                            uniqueMap[key] = nextIndex;

                            finalVertices.Add(vertices[vIdx]);
                            finalTexCoords.Add(tIdx >= 0 ? texCoords[tIdx] : Vector2.Zero);
                            finalNormals.Add(nIdx >= 0 ? normals[nIdx] : Vector3.Zero);

                            indices.Add(nextIndex);
                            nextIndex++;
                        }
                        else // already added the vertex, just reuse
                            indices.Add(index);
                    }
                    continue;
                }
            }

            Logger.Log(
                $"Loaded {LogColors.BW(path)}:\n" +
                $"    Vertice Count: {LogColors.BW(finalVertices.Count)}\n" +
                $"    Normal Count: {LogColors.BW(finalNormals.Count)}\n" +
                $"    TexCoord Count: {LogColors.BW(finalTexCoords.Count)}\n" +
                $"    Indice Count: {LogColors.BW(indices.Count)}\n" +
                $"    Triangle Count: {LogColors.BW(indices.Count / 3)}\n" +
                $"    In {LogColors.BG(Logger.EndTimingBlockFormatted())}"
                , LogLevel.Detail);

            return
                (
                finalVertices.ToArray(),
                finalNormals.ToArray(),
                finalTexCoords.ToArray(),
                indices.ToArray()
                );

        }
    }
}
