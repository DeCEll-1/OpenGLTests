using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenglTestConsole.classes
{
    public class Camera2D
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Zoom { get; set; } = 1.0f;

        public int screenWidth, screenHeight;

        public Camera2D(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix4.CreateScale(Zoom, Zoom, 1);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return
            Matrix4.CreateOrthographicOffCenter(
                -screenWidth / 2f, screenWidth / 2f,
                -screenHeight / 2f, screenHeight / 2f,
                -1f, 1f
            );
        }
    }

}
