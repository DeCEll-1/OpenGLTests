using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.impl.EFSs;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.Implementations.RenderScripts;
using OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;
using OpenglTestConsole.Classes.API.misc;
using OpenglTestConsole.Classes.Implementations.EFSs;
using OpenglTestConsole.Classes.API;

namespace OpenglTestConsole.Classes
{
    public class Main : GameWindow
    {
        public static Scene mainScene = new Scene();
        public List<EveryFrameScript> EveryFrameScripts { get; set; } = new List<EveryFrameScript>();
        public List<RenderScript> RenderScripts { get; set; } = new List<RenderScript>();
        private Camera Camera => Scene.Camera;

        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Scene.Camera = new Camera(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Scene.Camera.Position.Z = 3f;
            CursorState = CursorState.Grabbed;

            //Logger.Log($"{GL.GetInteger(GetPName.MaxVertexAttribs)}", LogLevel.Info);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            Scene.Lights.Add(new Light(new Vector3(5f, 5f, 5f), new Vector3(1.0f, 1.0f, 1.0f)));

            #region EFSs
            this.EveryFrameScripts.AddRange(
                [
                    new HandleMovement(),
                    new HandleMouse(),
                    new HandleZoom(),
                    new HandleResourceRefreshes(),
                ]
            );
            #endregion

            #region Render Scripts
            this.RenderScripts.AddRange(
                [
                    //new RenderCoordinates(),
                    ////new TestSphere(),
                    ////new TestCylinder(),
                    //new RenderStarscapeMap(0.1f),
                    //new RenderStarscapeConnections(0.1f),
                    //new TextRendering(),
                    new PhongTest(),
                    new TextureMaterialTests(),
                    new SquareTest(),
                    new RenderLight(),
                    new ComputeShaderTest(),
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
                script.Main = this;

                script.Init();
            }

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            mainScene.Render(renderScripts: RenderScripts, args: args);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        { // this straight up doesnt work
            base.OnFramebufferResize(e);
            Camera.screenWidth = e.Width;
            Camera.screenHeight = e.Height;
            GL.Viewport(0, 0, e.Width, e.Height);
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
                script.Main = this;
                script.Advance();
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }


    }
}
