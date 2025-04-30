using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class Transform
    {

        public Vector3 Position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public void UpdateMatrix()
        {
            _rotation = Rotation * new Vector3((float)(Math.PI / 180f));
        }
        public Vector3 Scale = Vector3.One;

        public Matrix4 GetModelMatrix()
        {
            Matrix4 rotation = Matrix4.CreateRotationX(_rotation.Y) *
                               Matrix4.CreateRotationY(_rotation.Z) *
                               Matrix4.CreateRotationZ(_rotation.X);
            return
            Matrix4.CreateScale(Scale) *
            rotation *
            Matrix4.CreateTranslation(Position) *
            Matrix4.Identity;
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
