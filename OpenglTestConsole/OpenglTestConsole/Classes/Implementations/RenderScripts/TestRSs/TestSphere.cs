using OpenglTestConsole.Classes.api.rendering;
using OpenglTestConsole.Classes.impl.rendering;
using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class TestSphere : RenderScript
    {
        private Sphere Sphere { get; set; }
        public override void Init()
        {
            this.Sphere = new Sphere(
                camera: this.Camera,
                stackCount: 16,
                sectorCount: 16,
                radius: 0.5f,
                color: new Vector4(1f, 1f, 1f, 1f)
            //texture: Main.Textures["Resources/Textures/sebestyen.png"]
            );

            this.Sphere.Transform.Position = new Vector3(-3f, 0f, 3f);
        }

        public override void Render()
        {
            Sphere.PrepareRender(MainInstance.light);
            Sphere.Render();
            Sphere.EndRender();
        }
    }
}
