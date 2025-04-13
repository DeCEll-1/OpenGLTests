using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.classes.api.rendering;
using OpenglTestConsole.classes.impl.EFSs;
using OpenglTestConsole.classes.impl.rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.RenderScripts;
using OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;

namespace OpenglTestConsole.classes
{
    public class Main : GameWindow
    {

        private Stopwatch Timer = new Stopwatch();
        public required Camera Camera { get; set; }
        public List<EveryFrameScript> EveryFrameScripts { get; set; } = new List<EveryFrameScript>();
        public List<RenderScript> RenderScripts { get; set; } = new List<RenderScript>();
        public static Dictionary<string, Texture> Textures { get; set; } = new();
        public static Dictionary<string, Shader> Shaders { get; set; } = new();
        public static Dictionary<string, FontJson> Fonts { get; set; } = new();
        public Light light = new Light(
            location: new Vector3(0f, 5f, 0f),
            color: new Vector4(1f, 1f, 1f, 0.9f),
            ambient: new Vector4(1f, 1f, 1f, 0.4f)
        );

        [SetsRequiredMembers]
        public Main(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Camera = new Camera(nativeWindowSettings.ClientSize.X, nativeWindowSettings.ClientSize.Y);
            Camera.Position.Z = 3f;
            CursorState = CursorState.Grabbed;

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            #region EFSs
            this.EveryFrameScripts.AddRange(
                [
                    new HandleMovement(),
                    new HandleMouse(),
                    new HandleZoom(),
                ]
            );
            #endregion

            #region Render Scripts
            this.RenderScripts.AddRange(
                [
                    new TestSphere(),
                    //new TestCylinder(),
                    //new RenderStarscapeMap(0.1f),
                    //new RenderStarscapeConnections(0.1f),
                    new RenderCoordinates(),
                    new TextRendering(),
                ]
            );
            #endregion

            #region Add Textures
            Textures.Add("Resources/Textures/PlaceHolder.png", new("Resources/Textures/PlaceHolder.png"));
            Textures.Add("Resources/Textures/sebestyen.png", new("Resources/Textures/sebestyen.png"));
            Textures.Add("Resources/Fonts/ComicSans.png", new("Resources/Fonts/ComicSans.png"));

            foreach (var tex in Textures)
                tex.Value.Init();
            #endregion

            #region Add Shaders

            Shaders.Add("default", new("Resources/Shaders/default.vert", "Resources/Shaders/default.frag"));
            Shaders.Add("greenBlink", new("Resources/Shaders/greenBlink.vert", "Resources/Shaders/greenBlink.frag"));

            Shaders.Add("objectMonoColor", new("Resources/Shaders/objectMonoColor.vert", "Resources/Shaders/objectMonoColor.frag"));
            Shaders.Add("objectTextured", new("Resources/Shaders/objectTextured.vert", "Resources/Shaders/objectTextured.frag"));

            Shaders.Add("texture", new("Resources/Shaders/texture.vert", "Resources/Shaders/texture.frag"));

            Shaders.Add("MCSDF", new("Resources/Shaders/MCSDF.vert", "Resources/Shaders/MCSDF.frag"));

            foreach (var shader in Shaders)
                shader.Value.Init();

            #endregion

            #region Add Fonts
            // i WİLL come back to fonts, i dont need it rn
            Fonts.Add("Resources/Fonts/ComicSans.json", MCSDFJSON.GetFontJson("Resources/Fonts/ComicSans.json"));
            #endregion


        }
        protected override void OnLoad()
        {
            base.OnLoad();


            GL.ClearColor(0.0f, 0.1f, 0.05f, 1.0f);

            foreach (var script in RenderScripts)
            {
                script.Timer = this.Timer;
                script.Camera = this.Camera;
                script.MainInstance = this;
                script.Init();
            }

            foreach (var script in EveryFrameScripts)
            {
                script.KeyboardState = this.KeyboardState;
                script.MouseState = this.MouseState;
                script.Camera = this.Camera;
                script.Main = this;
                script.Advance();
            }

            Timer = new Stopwatch();
            Timer.Start();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var script in RenderScripts)
            {
                script.Camera = this.Camera;
                script.Timer = this.Timer;
                script.MainInstance = this;
                script.Render();
            }

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
