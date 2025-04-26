using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class PhongTest : RenderScript
    {
        private Mesh Mesh { get; set; }
        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);
            PhongMaterial material = PhongMaterial.Pearl;

            this.Mesh = new Mesh(geometry, material);
        }

        public override void Render()
        {
            RenderMisc.EnableCullFace();
            Mesh.Render();
            RenderMisc.DisableCullFace();
        }
    }
}
