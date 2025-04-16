using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.impl.EFSs;
using OpenglTestConsole.Classes.impl.rendering;
using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.RenderScripts;
using OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;
using OpenglTestConsole.Classes.API.misc;

namespace OpenglTestConsole.Classes
{
    public class Main : GameWindow
    {

        private Stopwatch Timer = new Stopwatch();
        public required Camera Camera { get; set; }
        public List<EveryFrameScript> EveryFrameScripts { get; set; } = new List<EveryFrameScript>();
        public List<RenderScript> RenderScripts { get; set; } = new List<RenderScript>();
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

            //Logger.Log($"{GL.GetInteger(GetPName.MaxVertexAttribs)}", LogLevel.Info);

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
                    new RenderCoordinates(),
                    //new TestSphere(),
                    //new TestCylinder(),
                    //new TextRendering(),
                    //new RenderStarscapeMap(0.1f),
                    new RenderStarscapeConnections(0.1f),
                ]
            );
            #endregion


            #region Add Textures
            Resources.Textures.Add(ResourcePaths.Textures.PlaceHolder_png, new(ResourcePaths.Textures.PlaceHolder_png));
            Resources.Textures.Add(ResourcePaths.Textures.sebestyen_png, new(ResourcePaths.Textures.sebestyen_png));
            Resources.Textures.Add(ResourcePaths.Fonts.ComicSans_png, new(ResourcePaths.Fonts.ComicSans_png));

            Resources.Textures.Add(ResourcePaths.Textures.CoordinateSystem_png, new(ResourcePaths.Textures.CoordinateSystem_png));

            Resources.Textures.Add(ResourcePaths.Textures.RotationSystem_png, new(ResourcePaths.Textures.RotationSystem_png));

            foreach (var tex in Resources.Textures)
                tex.Value.Init();
            #endregion

            #region Add Shaders

            Resources.Shaders.Add(ResourcePaths.ShaderNames.defaultShader, new(ResourcePaths.ShaderFilePaths.default_vert, ResourcePaths.ShaderFilePaths.default_frag));

            Resources.Shaders.Add(ResourcePaths.ShaderNames.greenBlink, new(ResourcePaths.ShaderFilePaths.greenBlink_vert, ResourcePaths.ShaderFilePaths.greenBlink_frag));

            Resources.Shaders.Add(ResourcePaths.ShaderNames.objectMonoColor, new(ResourcePaths.ShaderFilePaths.objectMonoColor_vert, ResourcePaths.ShaderFilePaths.objectMonoColor_frag));

            Resources.Shaders.Add(ResourcePaths.ShaderNames.objectTextured, new(ResourcePaths.ShaderFilePaths.objectTextured_vert, ResourcePaths.ShaderFilePaths.objectTextured_frag));

            Resources.Shaders.Add(ResourcePaths.ShaderNames.texture, new(ResourcePaths.ShaderFilePaths.texture_vert, ResourcePaths.ShaderFilePaths.texture_frag));

            Resources.Shaders.Add(ResourcePaths.ShaderNames.MCSDF, new(ResourcePaths.ShaderFilePaths.MCSDF_vert, ResourcePaths.ShaderFilePaths.MCSDF_frag));

            // DO TEXTURE İNSTANCE RENDERİNG THİNG LATER

            Resources.Shaders.Add(ResourcePaths.ShaderNames.instancedRenderingMonoColor, new(ResourcePaths.ShaderFilePaths.instancedRenderingMonoColor_vert, ResourcePaths.ShaderFilePaths.instancedRenderingMonoColor_frag));

            foreach (var shader in Resources.Shaders)
                shader.Value.Init();

            #endregion

            #region Add Fonts
            // i WİLL come back to fonts, i dont need it rn
            Resources.Fonts.Add(ResourcePaths.Fonts.ComicSans_json, MCSDFJSON.GetFontJson(ResourcePaths.Fonts.ComicSans_json)!);
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
