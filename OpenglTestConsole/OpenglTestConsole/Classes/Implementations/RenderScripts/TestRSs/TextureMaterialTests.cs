using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Graphics.OpenGL;
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
            Mesh.Transform.Rotation.Y = 90;
            Mesh.Transform.UpdateMatrix();
            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }

        public override void Render()
        {
            Mesh.Render();
        }

        public override void OnResourceRefresh()
        {
            Mesh.Material = new TextureMaterial(Resources.Textures[ResourcePaths.Textures.cobble_stone_png]);
        }

    }
}
