using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        private Matrix4 cacheMatrix = Matrix4.Identity;

        public Transform()
        {
            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            _rotation = Rotation * new Vector3((float)(Math.PI / 180f));
            Matrix4 rotation =
                Matrix4.CreateRotationX(_rotation.Y)
                * Matrix4.CreateRotationY(_rotation.Z)
                * Matrix4.CreateRotationZ(_rotation.X);

            this.cacheMatrix =
                Matrix4.CreateScale(Scale)
                * rotation
                * Matrix4.CreateTranslation(Position)
                * Matrix4.Identity;
        }

        public Vector3 Scale = Vector3.One;

        public Matrix4 GetModelMatrix()
        {
            // DONE: you only need to reinitilise rotation when the rotation is updated, make this more efficent
            // you dont even need to remake the whole thing really, cache this thing cuh!

            return cacheMatrix;
        }

        public void Reset()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            UpdateMatrix();
        }
    }
}
