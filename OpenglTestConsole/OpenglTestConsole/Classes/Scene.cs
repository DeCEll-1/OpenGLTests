using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes
{
    public class Scene
    { // i am going for a threejs like api
        public static List<Light> Lights { get; set; } = new();
        public static Camera Camera { get; set; } = new Camera(800, 600);
        public Stopwatch Timer = new Stopwatch();
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
                script.Camera = Scene.Camera;
                script.Timer = this.Timer;
                script.Scene = this;

                script.Init();
            }
        }
        public void Render(List<RenderScript> renderScripts, FrameEventArgs args, Camera? camera = null)
        {
            if (camera != null)
                Camera = camera;

            foreach (RenderScript script in renderScripts)
            {
                script.args = args;
                script.Camera = Scene.Camera;
                script.Timer = this.Timer;
                script.Scene = this;

                script.Render();
            }
        }

    }
}
