using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class StandartMaterialTest : RenderScript
    {
        private Mesh Mesh;
        public override void Init()
        {
            //var geometry = new Sphere(16, 16, 1f);
            var geometry = new Cube(new(1f));
            var material = new TextureMaterial(Resources.Textures[ResourcePaths.Resources.Textures.Bell_Pepper_png]);
            //var material = new StandartMaterial();
            Mesh = new Mesh(geometry, material, name: "bell, pepper");
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
