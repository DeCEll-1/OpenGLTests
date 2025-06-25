using RGL.API;
using RGL.API.Rendering;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;
using RGL.Generated.Paths;

namespace RGL.Classes.Implementations.RenderScripts.TestRSs
{
    internal class TextureMaterialTests : RenderScript
    {
        private Mesh Mesh { get; set; }

        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);
            TextureMaterial material = new TextureMaterial(
                Resources.Textures[AppResources.Textures.cobble_stone_png]
            );

            this.Mesh = new Mesh(geometry, material, "Texture sphere");

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
                Resources.Textures[AppResources.Textures.cobble_stone_png]
            );
        }
    }
}
