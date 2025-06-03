using System.Diagnostics;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using OpenTK.Windowing.Common;

namespace OpenglTestConsole.Classes.API
{
    public class Scene
    { // i am going for a threejs like api
        public static List<Light> Lights { get; set; } = new();
        public static Camera Camera { get; set; } = new Camera(800, 600);
        public Stopwatch Timer = new Stopwatch();
        private List<List<Mesh>> renderList = [];
        private Cubemap _SkyboxMap = Resources.Cubemaps[ResourcePaths.Cubemaps.Sea.Name];
        public Cubemap SkyboxMap
        {
            get => _SkyboxMap; set
            { // update the skyboxes material when new skybox cubemap gets setten
                _SkyboxMap = value;
                Skybox.Material = new SkyboxMaterial(value);
            }
        }
        public Mesh Skybox { get; private set; }
        public void Add(Mesh mesh) => renderList.Add([mesh]);

        public void Add(List<Mesh> mesh) => renderList.Add(mesh);

        public Scene()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Init(List<RenderScript> renderScripts, Camera? camera = null)
        {
            Skybox = new(new SkyboxGeometry(), new SkyboxMaterial(SkyboxMap));
            Skybox.type = PrimitiveType.Triangles;

            if (camera != null)
                Camera = camera;

            foreach (RenderScript script in renderScripts)
            {
                script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.Init();
            }
        }

        public void Render(
            List<RenderScript> renderScripts,
            FrameEventArgs args,
            Camera? camera = null
        )
        {
            if (camera != null)
                Camera = camera;

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
            renderList.ForEach(scriptRenders => scriptRenders.ForEach(mesh => mesh?.Render()));

            // after render
            foreach (RenderScript script in renderScripts)
            {
                script.args = args; script.Camera = Camera; script.Timer = Timer; script.Scene = this;

                script.AfterRender();
            }
        }
    }
}
