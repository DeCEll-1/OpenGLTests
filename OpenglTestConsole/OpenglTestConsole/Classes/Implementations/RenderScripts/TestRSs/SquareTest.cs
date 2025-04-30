using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class SquareTest : RenderScript
    {
        private Mesh Mesh { get; set; }
        public override void Init()
        {
            //Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);

            Cube geometry = new Cube(new(1));

            PhongMaterial material = PhongMaterial.Pearl;

            this.Mesh = new Mesh(geometry, material);
            this.Mesh.Transform.Position.X -= 3;
            this.Mesh.Transform.UpdateMatrix();
            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }

        public override void Render()
        {
            Mesh.Render();
        }

        public override void OnResourceRefresh()
        {
            Mesh.Material = PhongMaterial.Pearl;
        }
    }
}
