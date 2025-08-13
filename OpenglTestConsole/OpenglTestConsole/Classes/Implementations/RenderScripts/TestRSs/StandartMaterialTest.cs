using RGL.API;
using RGL.API.Rendering;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.Classes.Implementations.RenderScripts.TestRSs
{
    public class StandartMaterialTest : RenderScript
    {
        private Mesh Mesh;
        public override void Init()
        {
            //var geometry = new Sphere(16, 16, 1f);
            var geometry = new Cube(new(1f));
            var material = new TextureMaterial(Resources.Textures[AppResources.Textures.Bell_Pepper_png]);
            //var material = new StandartMaterial();
            Mesh = new Mesh(geometry, material, name: "bell, pepper");
            Mesh.IsTransparent = true;
            //Mesh.CapsToEnable.Add(EnableCap.CullFace);

            Scene.Add(Mesh);
        }

        public override void Advance()
        {
        }

        public override void OnResourceRefresh()
        {
            base.OnResourceRefresh();
        }
    }
}
