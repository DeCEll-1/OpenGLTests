using OpenTK.Mathematics;
using RGL.API.Rendering.MeshClasses;

namespace RGL.API.Rendering.Geometries
{
    public abstract class Geometry3D
    {
        public int size;
        #region length
        public int IndicesLength { get => Indices.Length; }
        public int VerticesLength { get => Vertices.Length; }
        public int NormalsLength { get => Normals.Length; }
        public int TexCoordsLength { get => TexCoords.Length; }
        #endregion

        public uint[] Indices = [];

        public Vector3[] Vertices = Array.Empty<Vector3>();
        public Vector3[] Normals = Array.Empty<Vector3>();
        public Vector2[] TexCoords = Array.Empty<Vector2>();

        public virtual void Apply(BufferManager BufferManager)
        {
            size = VerticesLength; // almost forgor this lmao
            BufferManager.SetVector3(Vertices, 0);
            BufferManager.SetVector3(Normals, 1);
            BufferManager.SetVector2(TexCoords, 2);
            BufferManager.SetIndices(Indices);
        }

        public void ApplyTransformation(Matrix4 transform)
        {
            // Transform vertex positions
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vector3.TransformPosition(Vertices[i], transform);
            }

            // Transform normals (ignore translation)
            for (int i = 0; i < Normals.Length; i++)
            {
                Normals[i] = Vector3.TransformNormal(Normals[i], transform);
                Normals[i] = Normals[i].Normalized();
            }
        }

        public static MergedGeometry MergeGeometries(Geometry3D[] geometries)
        {
            MergedGeometry mergedGeometry = new MergedGeometry();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<uint> indices = new List<uint>();
            uint indexOffset = 0;
            foreach (var geometry in geometries)
            {
                vertices.AddRange(geometry.Vertices);
                normals.AddRange(geometry.Normals);
                texCoords.AddRange(geometry.TexCoords);
                indices.AddRange(geometry.Indices.Select(i => i + indexOffset));
                indexOffset += (uint)geometry.Vertices.Length;
            }
            mergedGeometry.Vertices = vertices.ToArray();
            mergedGeometry.Normals = normals.ToArray();
            mergedGeometry.TexCoords = texCoords.ToArray();
            mergedGeometry.Indices = indices.ToArray();
            return mergedGeometry;
        }

        public void Subdivide()
        {
            var newVertices = new List<Vector3>(Vertices);
            var newNormals = new List<Vector3>(Normals);
            var newTexCoords = new List<Vector2>(TexCoords);
            var newIndices = new List<uint>();

            // For edge midpoint caching: (minIndex, maxIndex) -> newVertexIndex
            var midpointCache = new Dictionary<(uint, uint), uint>();

            for (int i = 0; i < Indices.Length; i += 3)
            {
                uint i0 = Indices[i];
                uint i1 = Indices[i + 1];
                uint i2 = Indices[i + 2];

                // Get or create midpoint vertices for each edge
                uint m0 = GetOrCreateMidpoint(i0, i1, newVertices, newNormals, newTexCoords, midpointCache);
                uint m1 = GetOrCreateMidpoint(i1, i2, newVertices, newNormals, newTexCoords, midpointCache);
                uint m2 = GetOrCreateMidpoint(i2, i0, newVertices, newNormals, newTexCoords, midpointCache);

                // Add 4 new triangles
                newIndices.AddRange(new uint[] {
            i0, m0, m2,
            i1, m1, m0,
            i2, m2, m1,
            m0, m1, m2
        });
            }

            Vertices = newVertices.ToArray();
            Normals = newNormals.ToArray();
            TexCoords = newTexCoords.ToArray();
            Indices = newIndices.ToArray();
        }

        // Helper: returns the index of the midpoint, creating if needed
        private uint GetOrCreateMidpoint(
            uint iA, uint iB,
            List<Vector3> vertices,
            List<Vector3> normals,
            List<Vector2> texCoords,
            Dictionary<(uint, uint), uint> cache)
        {
            var key = iA < iB ? (iA, iB) : (iB, iA);
            if (cache.TryGetValue(key, out uint midIndex))
                return midIndex;

            // Interpolate position
            Vector3 vA = vertices[(int)iA], vB = vertices[(int)iB];
            Vector3 midpoint = (vA + vB) * 0.5f;
            vertices.Add(midpoint);

            // Interpolate normal
            Vector3 nA = normals[(int)iA], nB = normals[(int)iB];
            Vector3 nMid = ((nA + nB) * 0.5f).Normalized();
            normals.Add(nMid);

            // Interpolate texcoord
            Vector2 tA = texCoords[(int)iA], tB = texCoords[(int)iB];
            Vector2 tMid = (tA + tB) * 0.5f;
            texCoords.Add(tMid);

            midIndex = (uint)(vertices.Count - 1);
            cache[key] = midIndex;
            return midIndex;
        }


    }
}
