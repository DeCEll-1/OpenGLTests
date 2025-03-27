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
        public required Camera2D Camera { get; set; }
        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Camera = new Camera2D(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Mesh = new Mesh(this.Camera);

        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //  new Vector3(-1.0f, 1.0f, 0f),
            //  new Vector3(1.0f, 1.0f, 0f),
            //  new Vector3(-1.0f, -1.0f, 0f),
            //  new Vector3(1.0f, -1.0f, 0f)

            Mesh = new Mesh(this.Camera);
            Mesh.size = 4;
            Mesh.SetVector3(
                Square.GetSquare(400, 300),
                0);

            Mesh.InitShader("shaders/shader.vert", "shaders/shader.frag");

            Mesh.Shader.Use();

            timer = new Stopwatch();
            timer.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

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

            float speed = 10f;
            float delta = (float)args.Time;

            speed *= delta;

            if (KeyboardState.IsKeyDown(Keys.Right)) Mesh.Transform.Position.X += speed;
            if (KeyboardState.IsKeyDown(Keys.Left)) Mesh.Transform.Position.X -= speed;
            if (KeyboardState.IsKeyDown(Keys.Up)) Mesh.Transform.Position.Y += speed;
            if (KeyboardState.IsKeyDown(Keys.Down)) Mesh.Transform.Position.Y -= speed;



            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

    }
}
