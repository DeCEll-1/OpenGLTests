using OpenTK.Mathematics;

namespace RGL.API.Rendering.Geometries
{
    public class SkyboxGeometry : Geometry3D
    {
        public SkyboxGeometry()
        {
            Init();
        }
        private void Init()
        {
            (Vertices, Normals, TexCoords, Indices) = GetSkybox();
        }

        private (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices
        ) GetSkybox()
        {
            return (
                vertices:
                [
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),

                    new(-1.0f, -1.0f,  1.0f),
                    new(-1.0f, -1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new(-1.0f, -1.0f,  1.0f),

                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),

                    new(-1.0f, -1.0f,  1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f, -1.0f,  1.0f),
                    new(-1.0f, -1.0f,  1.0f),

                    new(-1.0f,  1.0f, -1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new(-1.0f,  1.0f, -1.0f),

                    new(-1.0f, -1.0f, -1.0f),
                    new(-1.0f, -1.0f,  1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new(-1.0f, -1.0f,  1.0f),
                    new( 1.0f, -1.0f,  1.0f)
                ],
                normals: [],
                texCoords: [], // same as the vertex positions
                indices: []
                );
        }
    }
}
