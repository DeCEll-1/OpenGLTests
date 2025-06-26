using System.Diagnostics.CodeAnalysis;
using ImGuiNET;
using RGL.Classes.API.Rendering;
using RGL.API.Rendering.Geometries;
using RGL.API.SceneFolder;
using RGL.Classes.impl.EFSs;
using RGL.Classes.Implementations.EFSs;
using RGL.Classes.Implementations.RenderScripts;
using RGL.Classes.Implementations.RenderScripts.TestRSs;
using RGL.Generated.Paths;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API;
using RGL.API.Rendering;
using RGL.API.ImGuiHelpers;
using RGL.API.Misc;

namespace RGL.Classes
{
    public class Main : GameWindow
    {
        ImGuiController _controller;

        public static Scene mainScene = new Scene();
        public List<EveryFrameScript> EveryFrameScripts { get; set; } =
            new List<EveryFrameScript>();
        public List<RenderScript> RenderScripts { get; set; } = new List<RenderScript>();
        private Camera Camera => mainScene.Camera;

        [SetsRequiredMembers]
        public Main(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings
        )
            : base(gameWindowSettings, nativeWindowSettings)
        {
            ResourceController.Init(typeof(AppResources));
            mainScene.Camera = new Camera();
            mainScene.Camera.Position.Z = 3f;
            mainScene.Camera.screenWidth = APISettings.Resolution.X;
            mainScene.Camera.screenHeight = APISettings.Resolution.Y;
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
                    new DisplaySceneInfo(),
                    new WindowSizeSettings(),
                    new RenderFumo(),
                ]
            );
            #endregion

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);

            mainScene.Resolution = APISettings.Resolution;
            mainScene.Init(renderScripts: RenderScripts, everyFrameScripts: EveryFrameScripts, this);
            mainScene.SkyboxCubeMap = Resources.Cubemaps[AppResources.Cubemaps.Sea.Name];




            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if (!IsFocused) // check to see if the window is focused
                return;

            _controller.Update(this, (float)args.Time);

            // rendered everything we need to render
            mainScene.Render(args: args, window: this);
            RenderMisc.RenderSceneToScreen(mainScene);

            _controller.Render();

            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) // check to see if the window is focused
                return;

            mainScene.RunEveryFrameScripts(args: args, window: this);

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
            if (APISettings.Resolution == new Vector2(e.Width, e.Height))
                return;

            APISettings.Resolution = new(e.Width, e.Height);
            mainScene.Resolution = new(e.Width, e.Height);
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
