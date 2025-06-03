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
    }
}
