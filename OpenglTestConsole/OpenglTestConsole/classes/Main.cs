using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenglTestConsole.classes
{
    public class Main : GameWindow
    {
        private Stopwatch timer = new Stopwatch();
        public required Mesh Mesh { get; set; }
        public required Camera Camera { get; set; }

        private float _sensitivity = 0.2f;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Camera = new Camera(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Camera.Position.Z = 3f;

            Mesh = new Mesh(this.Camera);

            CursorState = CursorState.Grabbed;

        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            Mesh = new Mesh(this.Camera);
            Mesh.size = 4;
            Mesh.SetVector3(
                //Square.GetSquare(400, 300),
                new Vector3[]
                {
                    new Vector3(-0.5f, 0.5f, 0f),
                    new Vector3(0.5f, 0.5f, 0f),
                    new Vector3(-0.5f, -0.5f, 0f),
                    new Vector3(0.5f, -0.5f, 0f)
                },
                0
            );


            Mesh.InitShader("shaders/shader.vert", "shaders/shader.frag");

            Mesh.Shader.Use();

            timer = new Stopwatch();
            timer.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Mesh.Transform.Position = new Vector3(0f, 0f, 0f);

            Mesh.Shader.Use();

            Mesh.Shader.SetFloat("t", (float)timer.Elapsed.TotalSeconds);

            Mesh.Render();

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            Camera.screenWidth = e.Width;
            Camera.screenHeight = e.Height;
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            float speed = 1f;
            float delta = (float)args.Time;

            speed *= delta;

            float radians = MathHelper.DegreesToRadians(Camera.Yaw);

            Vector3 forward = new Vector3((float)Math.Cos(MathHelper.DegreesToRadians(Camera.Yaw)), 0,
                                          (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Yaw)));

            Vector3 right = new Vector3((float)Math.Cos(MathHelper.DegreesToRadians(Camera.Yaw) + MathHelper.PiOver2), 0,
                                        (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Yaw) + MathHelper.PiOver2));
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

            Camera.Position += movement;

            if (KeyboardState.IsKeyDown(Keys.Right)) Mesh.Transform.Position.X += speed;
            if (KeyboardState.IsKeyDown(Keys.Left)) Mesh.Transform.Position.X -= speed;
            if (KeyboardState.IsKeyDown(Keys.Up)) Mesh.Transform.Position.Y += speed;
            if (KeyboardState.IsKeyDown(Keys.Down)) Mesh.Transform.Position.Y -= speed;

            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Camera.Yaw += deltaX * _sensitivity;
                Camera.Pitch -= deltaY * _sensitivity; // Reversed since y-coordinates range from bottom to top
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

    }
}
