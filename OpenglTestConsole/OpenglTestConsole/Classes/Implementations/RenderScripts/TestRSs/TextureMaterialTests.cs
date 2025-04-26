using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class TextureMaterialTests : RenderScript
    {
        private Mesh Mesh { get; set; }
        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);
            TextureMaterial material = new TextureMaterial(Resources.Textures[ResourcePaths.Textures.cobble_stone_png]);

            this.Mesh = new Mesh(geometry, material);

            Mesh.Transform.Position.X += 3;
            Mesh.Transform.SetRotation(y: 90);
        }

        public override void Render()
        {
            RenderMisc.EnableCullFace();
            Mesh.Render();
            RenderMisc.DisableCullFace();
        }
    }
}
