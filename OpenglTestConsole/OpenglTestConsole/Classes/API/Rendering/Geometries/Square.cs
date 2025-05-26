using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    internal class Square : Geometry3D
    {
        public Vector2 shape { get; private set; }

        public Square(Vector2 shape)
        {
            this.shape = shape;
            Init();
        }

        private void Init()
        {
            (this.Vertices, this.Normals, this.TexCoords, this.Indices) = GetPlane();
        }

        public (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices
        ) GetPlane()
        {
            // make variables for quick access as we are using floats and all deez are returning doubles
            // list because itd be annoying to use arrays from the start
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            float x = shape.X / 2f;
            float y = shape.Y / 2f;
            vertices.Add(new Vector3(-x, -y, 0));
            vertices.Add(new Vector3(x, -y, 0));
            vertices.Add(new Vector3(x, y, 0));
            vertices.Add(new Vector3(-x, y, 0));

            normals.Add(new Vector3(0, 0, 1));
            normals.Add(new Vector3(0, 0, 1));
            normals.Add(new Vector3(0, 0, 1));
            normals.Add(new Vector3(0, 0, 1));

            texCoords.Add(new Vector2(0, 0));
            texCoords.Add(new Vector2(1, 0));
            texCoords.Add(new Vector2(1, 1));
            texCoords.Add(new Vector2(0, 1));

            return (
                vertices.ToArray(),
                normals.ToArray(),
                texCoords.ToArray(),
                new uint[] {
                    0, 1, 2,
                    2, 3, 0
                }
            );
        }
    }
}
