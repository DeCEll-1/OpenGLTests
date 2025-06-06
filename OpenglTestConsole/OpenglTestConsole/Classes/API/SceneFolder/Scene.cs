using System.Diagnostics;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using OpenTK.Windowing.Common;

namespace OpenglTestConsole.Classes.API.SceneFolder
{
    public partial class Scene
    { // i am going for a threejs like api
        public Scene()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Init(List<RenderScript> renderScripts, Camera? camera = null)
        {
            #region skybox
            Skybox = new(new SkyboxGeometry(), new SkyboxMaterial(SkyboxMap));
            Skybox.type = PrimitiveType.Triangles;
            #endregion

            #region post processing

            InitPostProcesses();
            #endregion

            #region render scripts
            if (camera != null)
                Camera = camera;
            foreach (RenderScript script in renderScripts)
            {
                script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Init();
            }
            #endregion
        }

        public void Render(
            List<RenderScript> renderScripts,
            FrameEventArgs args,
            Camera? camera = null
        )
        {
            // we will render everything to our main fbo
            MainFBO.Bind();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            #region main render
            if (camera != null)
                Camera = camera
                    ;
            GL.DepthFunc(DepthFunction.Lequal);
            Skybox.Render();
            GL.DepthFunc(DepthFunction.Less);

            foreach (RenderScript script in renderScripts)
            {
                // advance the render scripts
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Advance();
            }

            // before render
            foreach (RenderScript script in renderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.BeforeRender();
            }

            // render the meshes added by the render scripts
            Meshes.ForEach(scriptRenders => scriptRenders.ForEach(mesh => mesh?.Render()));

            // after render
            foreach (RenderScript script in renderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.AfterRender();
            }
            #endregion

            // then unbind it
            MainFBO.Unbind();

            HandlePostProcesses();

        }
    }
}
