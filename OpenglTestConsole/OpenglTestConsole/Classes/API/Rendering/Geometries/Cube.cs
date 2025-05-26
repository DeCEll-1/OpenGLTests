using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    internal class Cube : Geometry3D
    {
        public Vector3 shape { get; private set; }

        public Cube(Vector3 shape)
        {
            this.shape = shape;
            Init();
        }

        private void Init()
        {
            (this.Vertices, this.Normals, this.TexCoords, this.Indices) = GetCube();
        }

        private (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices
        ) GetCube()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<uint> indices = new List<uint>();
            float x = shape.X;
            float y = shape.Y;
            float z = shape.Z;

            float xOffset = shape.X / 2f;
            float yOffset = shape.Y / 2f;
            float zOffset = shape.Z / 2f;

            Square front = new Square(new Vector2(x, y));
            Square back = new Square(new Vector2(x, y));
            Square left = new Square(new Vector2(z, y));
            Square right = new Square(new Vector2(z, y));
            Square top = new Square(new Vector2(x, z));
            Square bottom = new Square(new Vector2(x, z));

            Transform dummy = new Transform();
            dummy.Position.Z = -zOffset; // front
            dummy.Rotation.Z = 180;
            dummy.UpdateMatrix();
            front.ApplyTransformation(dummy.GetModelMatrix());
            dummy.Reset();

            dummy.Position.Z = zOffset;
            dummy.UpdateMatrix();
            back.ApplyTransformation(dummy.GetModelMatrix());
            dummy.Reset();

            dummy.Position.X = -xOffset;
            dummy.Rotation.Z = -90;
            dummy.UpdateMatrix();
            left.ApplyTransformation(dummy.GetModelMatrix());
            dummy.Reset();

            dummy.Position.X = xOffset;
            dummy.Rotation.Z = 90;
            dummy.UpdateMatrix();
            right.ApplyTransformation(dummy.GetModelMatrix());
            dummy.Reset();

            dummy.Position.Y = yOffset;
            dummy.Rotation.Y = -90;
            dummy.UpdateMatrix();
            top.ApplyTransformation(dummy.GetModelMatrix());
            dummy.Reset();

            dummy.Position.Y = -yOffset;
            dummy.Rotation.Y = 90;
            dummy.UpdateMatrix();
            bottom.ApplyTransformation(dummy.GetModelMatrix());

            MergedGeometry merged = Geometry3D.MergeGeometries(
                [front, back, left, right, top, bottom]
            );

            vertices.AddRange(merged.Vertices);
            normals.AddRange(merged.Normals);
            texCoords.AddRange(merged.TexCoords);
            indices.AddRange(merged.Indices);

            return (vertices.ToArray(), normals.ToArray(), texCoords.ToArray(), indices.ToArray());
        }
    }
}
