using RGL.API.Rendering;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Textures;
using System.Diagnostics;

namespace RGL.API.SceneFolder
{
    public partial class Scene
    {
        public static Stopwatch Timer = new Stopwatch();
        static Scene()
        {
            Timer.Start();
        }

        private List<RenderScript> RenderScripts = [];


        public static List<Light> Lights { get; set; } = new();
        public static Camera Camera { get; set; } = new Camera();


        public Cubemap SkyboxCubeMap
        {
            get => field; set
            { // update the skyboxes material when new skybox cubemap gets setten
                field = value;

                if (Skybox == null)
                {
                    SkyboxMaterial mat = new(field);
                    SkyboxGeometry geometry = new SkyboxGeometry();

                    Skybox = new Mesh(geometry, mat);
                    Skybox.type = PrimitiveType.Triangles;

                }
                else
                    Skybox.Material = new SkyboxMaterial(field);


            }
        }
        public Mesh Skybox { get; private set; } = null;

        public List<List<Mesh>> Meshes { get; private set; } = [];
        public void Add(Mesh mesh) => Meshes.Add([mesh]);

        public void Add(List<Mesh> mesh) => Meshes.Add(mesh);
    }
}
