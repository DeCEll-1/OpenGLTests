using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes.impl.EFSs
{
    public class HandleMovement : EveryFrameScript
    {
        public override void Init()
        {
        }
        public override void Advance()
        {
            float speed = 10f;
            float delta = (float)args.Time;

            speed *= delta;

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
                speed *= 2f;

            float radians = MathHelper.DegreesToRadians(Camera.Yaw);

            Vector3 forward = new Vector3((float)Math.Cos(MathHelper.DegreesToRadians(Camera.Yaw)), 0,
                                          (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Yaw)));

            Vector3 right = new Vector3((float)Math.Cos(MathHelper.DegreesToRadians(Camera.Yaw) + MathHelper.PiOver2), 0,
                                        (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Yaw) + MathHelper.PiOver2));

            Vector3 up = Vector3.UnitY;

            // Movement vector
            Vector3 movement = Vector3.Zero;

            // Move based on key presses
            if (KeyboardState.IsKeyDown(Keys.W))
                movement += forward * speed;
            if (KeyboardState.IsKeyDown(Keys.S))
                movement -= forward * speed;
            if (KeyboardState.IsKeyDown(Keys.D))
                movement += right * speed;
            if (KeyboardState.IsKeyDown(Keys.A))
                movement -= right * speed;
            if (KeyboardState.IsKeyDown(Keys.E) || KeyboardState.IsKeyDown(Keys.Space))
                movement += up * speed;
            if (KeyboardState.IsKeyDown(Keys.Q) || KeyboardState.IsKeyDown(Keys.LeftControl))
                movement -= up * speed;

            Camera.Position += movement;
        }
    }
}
