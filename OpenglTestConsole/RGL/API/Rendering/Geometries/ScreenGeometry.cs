using OpenTK.Mathematics;

namespace RGL.API.Rendering.Geometries
{
    public class ScreenGeometry : Geometry3D
    {
        public ScreenGeometry()
        {
            Init();
        }
        private void Init()
        {
            (Vertices, Normals, TexCoords, Indices) = GetSquare();
        }

        private (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices
        ) GetSquare()
        {
            return (
                vertices:
                [
                   new(-1, 1, 0 ),
                   new(1, 1, 0  ),
                   new(-1, -1, 0),
                   new(1, -1, 0 ),
                ],
                normals: [],
                texCoords:
                [
                    new(0, 1),
                    new(1, 1),
                    new(0, 0),
                    new(1, 0),
                ],
                indices:
                [
                    0, 1, 2,
                    1, 2, 3
                ]
                );
        }
    }
}
