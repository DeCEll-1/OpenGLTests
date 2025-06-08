using System.Diagnostics.CodeAnalysis;
using ImGuiNET;
using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.ImGuiHelpers;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.SceneFolder;
using OpenglTestConsole.Classes.impl.EFSs;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenglTestConsole.Classes.Implementations.EFSs;
using OpenglTestConsole.Classes.Implementations.RenderScripts;
using OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs;
using OpenglTestConsole.Generated.Paths;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenglTestConsole.Classes
{
    public class Main : GameWindow
    {
        ImGuiController _controller;

        public static Scene mainScene = new Scene();
        public List<EveryFrameScript> EveryFrameScripts { get; set; } =
            new List<EveryFrameScript>();
        public List<RenderScript> RenderScripts { get; set; } = new List<RenderScript>();
        private Camera Camera => Scene.Camera;

        [SetsRequiredMembers]
        public Main(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings
        )
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Scene.Camera = new Camera();
            Scene.Camera.Position.Z = 3f;
            CursorState = CursorState.Grabbed;

            //Logger.Log($"{GL.GetInteger(GetPName.MaxVertexAttribs)}", LogLevel.Info);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            Scene.Lights.Add(new Light(new Vector3(-70f, 50f, -50f), new Vector3(1.0f, 1.0f, 1.0f)));

            #region EFSs
            this.EveryFrameScripts.AddRange(
                [
                    new HandleMovement(),
                    new HandleMouse(),
                    new HandleZoom(),
                    new HandleResourceRefreshes(),
                    new HandlePrintScreen(),
                ]
            );
            #endregion

            #region Render Scripts
            this.RenderScripts.AddRange(
                [
                    new PhongTest(),
                    new TextureMaterialTests(),
                    new SquareTest(),
                    new RenderLight(),
                    new ComputeShaderTest(),
                    new StandartMaterialTest(),
                    new PostProcessingTest(),
                    new HandleImGuiAppInfo(),
                    new WindowSizeSettings(),
                    new RenderFumo(),
                ]
            );
            #endregion

            ResourceController.Init();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);

            mainScene.Init(renderScripts: RenderScripts);

            foreach (var script in EveryFrameScripts)
            {
                script.KeyboardState = this.KeyboardState;
                script.MouseState = this.MouseState;
                script.Camera = Scene.Camera;
                script.MainInstance = this;

                script.Init();
            }


            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _controller.Update(this, (float)args.Time);

            // rendered everything we need to render
            mainScene.Render(renderScripts: RenderScripts, args: args);

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

            _controller.Render();

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) // check to see if the window is focused
                return;

            foreach (var script in EveryFrameScripts)
            {
                script.args = args;
                script.KeyboardState = this.KeyboardState;
                script.MouseState = this.MouseState;
                script.Camera = this.Camera;
                script.MainInstance = this;
                script.Advance();
            }

            OpenTK.Graphics.OpenGL.ErrorCode error = GL.GetError();
            if (error != OpenTK.Graphics.OpenGL.ErrorCode.NoError)
            {
                Logger.LogWithoutGLErrorCheck(error.ToString(), LogLevel.Error);
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }


        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            if (Settings.Resolution == new Vector2(e.Width, e.Height))
                return;

            Settings.Resolution = new(e.Width, e.Height);
            // Update the opengl viewport
            GL.Viewport(0, 0, e.Width, e.Height);

            // Tell ImGui of the new size
            _controller.WindowResized(e.Width, e.Height);

            mainScene.UpdateFBOs();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);


            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }
    }
}
