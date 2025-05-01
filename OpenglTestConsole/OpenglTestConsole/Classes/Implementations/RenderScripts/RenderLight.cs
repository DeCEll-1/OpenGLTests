using OpenglTestConsole.Classes.API;
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

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class RenderLight : RenderScript
    {
        private Mesh Mesh;
        public override void Init()
        {
            this.Mesh = new Mesh(new Sphere(16, 16, 1f), PhongMaterial.Silver);
            this.Mesh.Transform.Position = Scene.Lights[0].Position;
            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }

        public override void Render()
        {
            Mesh.Render();
        }
        public override void OnResourceRefresh()
        {
            this.Mesh.Material = PhongMaterial.Silver;
        }
    }
}
