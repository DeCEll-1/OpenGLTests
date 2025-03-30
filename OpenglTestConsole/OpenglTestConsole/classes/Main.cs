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
        public required Sphere Sphere { get; set; }
        public required Mesh Square { get; set; }
        public required Mesh SquareTextured { get; set; }
        public required Camera Camera { get; set; }
        public Texture TestTexture { get; set; } = new Texture("textures/PlaceHolder.png");

        private float _sensitivity = 0.2f;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        public Light light = new Light(new Vector3(5.0f), new Vector3(1.0f));

        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Camera = new Camera(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Camera.Position.Z = 3f;
            CursorState = CursorState.Grabbed;

            this.TestTexture.Init();


        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            #region square
            Square = new Mesh(this.Camera);
            Square.size = 4;
            Square.SetVector3(
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
            Square.InitShader("shaders/shader.vert", "shaders/shader.frag");
            Square.Shader.Use();
            #endregion
            #region square Textured
            SquareTextured = new Mesh(this.Camera);
            SquareTextured.size = 4;
            SquareTextured.SetVector3(
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
            SquareTextured.SetVector2(
                [
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f)
                ],
                1
            );
            SquareTextured.InitShader("shaders/texture.vert", "shaders/texture.frag");
            SquareTextured.Shader.Use();
            #endregion
            #region sphere
            this.Sphere = new Sphere(
                stackCount: 16,
                sectorCount: 16,
                radius: 0.5f,
                camera: this.Camera,
                texture: new Texture("textures/sebestyen.png")
            );
            this.Sphere.Shader.Use();
            this.Sphere.Transform.Position = new Vector3(0f, -1.5f, 2f);
            #endregion

            timer = new Stopwatch();
            timer.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            #region square
            Square.Transform.Position = Vector3.UnitZ * 3f;
            Square.Shader.Use();
            Square.Shader.SetFloat("t", (float)timer.Elapsed.TotalSeconds);
            Square.Render();
            #endregion square

            #region square texture
            SquareTextured.Transform.Position = Vector3.UnitX * 3f;
            //SquareTextured.Transform.SetRotation(x: 90f);
            SquareTextured.Shader.Use();
            SquareTextured.Shader.SetTexture("tex", TestTexture, TextureUnit.Texture0);
            SquareTextured.Render();
            #endregion

            Sphere.Render(light: light);

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

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
                speed *= 2f;

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

            if (KeyboardState.IsKeyDown(Keys.Right)) Sphere.Transform.Position.X += speed;
            if (KeyboardState.IsKeyDown(Keys.Left)) Sphere.Transform.Position.X -= speed;
            if (KeyboardState.IsKeyDown(Keys.Up)) Sphere.Transform.Position.Y += speed;
            if (KeyboardState.IsKeyDown(Keys.Down)) Sphere.Transform.Position.Y -= speed;

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
