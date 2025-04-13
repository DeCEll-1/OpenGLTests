using OpenglTestConsole.classes.impl.rendering;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class TestCylinder : RenderScript
    {
        Cylinder Cylinder;
        public override void Init()
        {
            this.Cylinder = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 16,
                Radius: 0.5f,
                Height: 1.0f,
                color: new Vector4(1f, 1f, 1f, 1f)
            );

            this.Cylinder.Transform.Position = new Vector3(3f, 0f, 3f);

        }

        public override void Render()
        {
            Cylinder.PrepareRender(MainInstance.light);
            Cylinder.Render();
            Cylinder.EndRender();
        }
    }
}
