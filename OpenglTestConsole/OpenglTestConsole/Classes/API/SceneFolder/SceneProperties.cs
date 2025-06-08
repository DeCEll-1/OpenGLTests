using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.SceneFolder
{
    public partial class Scene
    {
        public static List<Light> Lights { get; set; } = new();
        public static Camera Camera { get; set; } = new Camera();
        public static Stopwatch Timer = new Stopwatch();
        static Scene()
        {
            Timer.Start();
        }
        public List<List<Mesh>> Meshes { get; private set; } = [];
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
        public void Add(Mesh mesh) => Meshes.Add([mesh]);

        public void Add(List<Mesh> mesh) => Meshes.Add(mesh);
    }
}
