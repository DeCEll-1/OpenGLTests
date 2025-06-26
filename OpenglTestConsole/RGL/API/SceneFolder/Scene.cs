using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API.Rendering;

namespace RGL.API.SceneFolder
{
    public partial class Scene
    { // i am going for a threejs like api
        public Scene()
        {
        }

        public void Init(List<RenderScript> renderScripts, List<EveryFrameScript> everyFrameScripts, GameWindow window)
        {

            this.RenderScripts = renderScripts;
            this.EveryFrameScripts = everyFrameScripts;

            #region post processing
            InitPostProcesses();
            #endregion

            foreach (RenderScript script in RenderScripts)
            {
                script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Init();
            }

            foreach (var script in EveryFrameScripts)
            {
                script.KeyboardState = window.KeyboardState;
                script.MouseState = window.MouseState;
                script.Camera = this.Camera;
                script.Window = window;

                script.Init();
            }
        }

        public void Render(
            FrameEventArgs args,
            GameWindow window
        )
        {
            GL.Viewport(0, 0, Resolution.X, Resolution.Y);

            // we will render everything to our main fbo
            MainFBO.Bind();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region main render

            GL.DepthFunc(DepthFunction.Lequal);
            Skybox?.Render(this);
            GL.DepthFunc(DepthFunction.Less);

            foreach (RenderScript script in RenderScripts)
            {
                // advance the render scripts
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this; script.Window = window;

                script.Advance();
            }

            // before render
            foreach (RenderScript script in RenderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this; script.Window = window;

                script.BeforeRender();
            }

            // render the meshes added by the render scripts
            Meshes.ForEach(scriptRenders => scriptRenders.ForEach(mesh => mesh?.Render(this)));

            // after render
            foreach (RenderScript script in RenderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this; script.Window = window;

                script.AfterRender();
            }
            #endregion


            RenderScripts = RenderScripts.FindAll(scriptRenders => !scriptRenders.disposed);


            // then unbind it
            MainFBO.Unbind();

            HandlePostProcesses();

            GL.Viewport(0, 0, APISettings.Resolution.X, APISettings.Resolution.Y);
        }

        public void RunEveryFrameScripts(FrameEventArgs args, GameWindow window)
        {

            foreach (var script in EveryFrameScripts)
            {
                script.args = args;
                script.KeyboardState = window.KeyboardState;
                script.MouseState = window.MouseState;
                script.Camera = this.Camera;
                script.Window = window;
                script.Scene = this;
                script.Advance();
            }
        }
    }
}
