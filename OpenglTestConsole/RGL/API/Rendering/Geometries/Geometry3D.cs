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


    }
}
