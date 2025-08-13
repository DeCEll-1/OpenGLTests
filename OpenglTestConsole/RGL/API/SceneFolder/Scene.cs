using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RGL.API.Rendering;
using RGL.API.Rendering.Materials.PPMaterials;
using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;

namespace RGL.API.SceneFolder
{
    public partial class Scene
    { // i am going for a threejs like api
        public Scene() { }
        public Scene(Vector2i resolution, Cubemap skyboxCubemap = null)
        {
            if (skyboxCubemap != null)
                this.SkyboxCubeMap = skyboxCubemap;
            this.Camera = new Camera(resolution);
            this.Resolution = resolution;
        }

        public void Init(List<RenderScript> renderScripts, List<EveryFrameScript> everyFrameScripts, GameWindow window)
        {

            this.PostProcesses.Insert(0, new PostProcess(new WBOITCompositeMaterial(WBOITFBO)));
            this.RenderScripts = renderScripts;
            this.EveryFrameScripts = everyFrameScripts;

            #region post processing
            InitPostProcesses();
            #endregion

            foreach (RenderScript script in RenderScripts)
            {
                script.Camera = Camera; script.Timer = Timer; script.Scene = this; script.Window = window;

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


            RenderTransparentObjects();


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

        private void RenderTransparentObjects()
        {
            WBOITFBO.Bind();

            GL.DepthMask(false);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(0, BlendingFactorSrc.One, BlendingFactorDest.One);
            GL.BlendFunc(1, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcColor);
            GL.BlendEquation(BlendEquationMode.FuncAdd);

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearBuffer(ClearBuffer.Color, 0, [0f, 0f, 0f, 0f]); // accum
            GL.ClearBuffer(ClearBuffer.Color, 1, [1f]); // revealage = 1.0

            foreach (var scriptRenders in TransparentMeshes)
                foreach (var mesh in scriptRenders)
                    mesh?.Render(this);

            // restore state
            GL.DepthMask(true); // Re-enable depth writing
            GL.Disable(EnableCap.Blend); // Optional: if no blending needed next
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); // Restore default
            WBOITFBO.Unbind();
            MainFBO.Bind();
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
