using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Generated.Paths;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class TextureMaterialTests : RenderScript
    {
        private Mesh Mesh { get; set; }

        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);
            TextureMaterial material = new TextureMaterial(
                Resources.Textures[ResourcePaths.Resources.Textures.cobble_stone_png]
            );

            this.Mesh = new Mesh(geometry, material);

            Mesh.Transform.Position.X += 3;
            this.Mesh.Transform.UpdateMatrix();

            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);

            Scene.Add(Mesh);
        }

        public override void Advance()
        {
        }

        public override void OnResourceRefresh()
        {
            Mesh.Material = new TextureMaterial(
                Resources.Textures[ResourcePaths.Resources.Textures.cobble_stone_png]
            );
        }
    }
}
