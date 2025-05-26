using System.Diagnostics;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Windowing.Common;

namespace OpenglTestConsole.Classes.API
{
    public class Scene
    { // i am going for a threejs like api
        public static List<Light> Lights { get; set; } = new();
        public static Camera Camera { get; set; } = new Camera(800, 600);
        public Stopwatch Timer = new Stopwatch();
        private List<List<Mesh>> renderList = [];

        public void Add(Mesh mesh) => renderList.Add([mesh]);

        public void Add(List<Mesh> mesh) => renderList.Add(mesh);

        public Scene()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Init(List<RenderScript> renderScripts, Camera? camera = null)
        {
            if (camera != null)
                Camera = camera;

            foreach (RenderScript script in renderScripts)
            {
                script.Camera = Camera;
                script.Timer = Timer;
                script.Scene = this;

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

            foreach (RenderScript script in renderScripts)
            {
                // advance the render scripts
                script.args = args;
                script.Camera = Camera;
                script.Timer = Timer;
                script.Scene = this;

                script.Advance();
            }

            // before render
            foreach (RenderScript script in renderScripts)
            {
                // advance the render scripts
                script.args = args;
                script.Camera = Camera;
                script.Timer = Timer;
                script.Scene = this;

                script.BeforeRender();
            }

            // render the meshes added by the render scripts
            renderList.ForEach(scriptRenders => scriptRenders.ForEach(mesh => mesh?.Render()));

            // after render
            foreach (RenderScript script in renderScripts)
            {
                // advance the render scripts
                script.args = args;
                script.Camera = Camera;
                script.Timer = Timer;
                script.Scene = this;

                script.AfterRender();
            }
        }
    }
}
