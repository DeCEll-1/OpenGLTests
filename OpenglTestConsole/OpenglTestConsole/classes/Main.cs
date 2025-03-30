using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenglTestConsole.classes.api.rendering;
using OpenglTestConsole.classes.impl.EFSs;
using OpenglTestConsole.classes.impl.rendering;
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
        public required Camera Camera { get; set; }
        public List<EveryFrameScript> EveryFrameScripts { get; set; } = new List<EveryFrameScript>();
        
        public required Sphere Sphere { get; set; }
        public required Mesh Square { get; set; }
        public required Mesh SquareTextured { get; set; }
        public Texture TestTexture { get; set; } = new Texture("Textures/PlaceHolder.png");

        public Light light = new Light(new Vector3(5.0f), new Vector3(1.0f));

        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Camera = new Camera(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Camera.Position.Z = 3f;
            CursorState = CursorState.Grabbed;

            this.TestTexture.Init();

            this.EveryFrameScripts.AddRange(
                [
                    new HandleMovement(),
                    new HandleMouse(),
                    new HandleZoom(),
                ]
            );


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
            Square.InitShader("Shaders/shader.vert", "Shaders/shader.frag");
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
            SquareTextured.InitShader("Shaders/texture.vert", "Shaders/texture.frag");
            SquareTextured.Shader.Use();
            #endregion
            #region sphere
            this.Sphere = new Sphere(
                stackCount: 16,
                sectorCount: 16,
                radius: 0.5f,
                camera: this.Camera,
                texture: new Texture("Textures/sebestyen.png")
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

            foreach (var script in EveryFrameScripts)
            {
                script.args = args;
                script.KeyboardState = this.KeyboardState;
                script.MouseState = this.MouseState;
                script.Camera = this.Camera;
                script.Advance();
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }


    }
}
