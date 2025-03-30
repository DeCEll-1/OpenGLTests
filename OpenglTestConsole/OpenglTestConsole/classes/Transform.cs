using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Transform
    {

        public Vector3 Position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        public Vector3 Rotation
        {
            get
            {
                return _rotation * new Vector3((float)(180f / Math.PI));
            }
            set
            {
                _rotation = value * new Vector3((float)(Math.PI / 180f));
            }
        }
        public void SetRotation(float? x = null, float? y = null, float? z = null)
        {
            var rotation = this.Rotation;
            if (x != null)
                rotation.X = (float)x;
            if (y != null)
                rotation.Y = (float)y;
            if (z != null)
                rotation.Z = (float)z;

            this.Rotation = rotation;
        }
        public Vector3 Scale = Vector3.One;

        public Matrix4 GetModelMatrix()
        {

            Matrix4 rotation = Matrix4.CreateRotationX(_rotation.X) *
                               Matrix4.CreateRotationY(_rotation.Y) *
                               Matrix4.CreateRotationZ(_rotation.Z);

            return Matrix4.Identity *
                Matrix4.CreateScale(Scale) *
                rotation *
                Matrix4.CreateTranslation(Position);
        }

    }
}
