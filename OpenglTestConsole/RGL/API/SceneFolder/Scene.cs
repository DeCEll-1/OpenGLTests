using OpenTK.Windowing.Common;
using RGL.API.Rendering;

namespace RGL.API.SceneFolder
{
    public partial class Scene
    { // i am going for a threejs like api
        public Scene()
        {
        }

        public void Init(List<RenderScript> renderScripts, Camera? camera = null)
        {

            this.RenderScripts = renderScripts;

            #region post processing
            InitPostProcesses();
            #endregion

            #region render scripts
            if (camera != null)
                Camera = camera;
            foreach (RenderScript script in RenderScripts)
            {
                script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Init();
            }
            #endregion
        }

        public void Render(
            FrameEventArgs args,
            Camera? camera = null
        )
        {
            GL.Viewport(0, 0, APISettings.SceneResolution.X, APISettings.SceneResolution.Y);

            // we will render everything to our main fbo
            MainFBO.Bind();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region main render
            if (camera != null)
                Camera = camera;

            GL.DepthFunc(DepthFunction.Lequal);
            Skybox?.Render();
            GL.DepthFunc(DepthFunction.Less);

            foreach (RenderScript script in RenderScripts)
            {
                // advance the render scripts
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Advance();
            }

            // before render
            foreach (RenderScript script in RenderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.BeforeRender();
            }

            // render the meshes added by the render scripts
            Meshes.ForEach(scriptRenders => scriptRenders.ForEach(mesh => mesh?.Render()));

            // after render
            foreach (RenderScript script in RenderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.AfterRender();
            }
            #endregion

            RenderScripts = RenderScripts.FindAll(scriptRenders => !scriptRenders.dispose);


            // then unbind it
            MainFBO.Unbind();

            HandlePostProcesses();

            GL.Viewport(0, 0, APISettings.Resolution.X, APISettings.Resolution.Y);
        }
    }
}
