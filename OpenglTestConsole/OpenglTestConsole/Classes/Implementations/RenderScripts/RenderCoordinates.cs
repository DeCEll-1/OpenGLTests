using OpenglTestConsole.classes.impl.rendering;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class RenderCoordinates : RenderScript
    {
        private Cylinder[] axes = new Cylinder[3];
        private Sphere[] sides = new Sphere[3];
        public override void Init()
        {
            axes[0] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                color: new OpenTK.Mathematics.Vector4(1f, 0f, 0f, 1f)
            );
            axes[0].Transform.Rotation = new OpenTK.Mathematics.Vector3(0f, 90f, 0f);
            sides[0] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(1f, 0f, 0f, 1f)
            );
            sides[0].Transform.Position = new OpenTK.Mathematics.Vector3(0f, 1f, 0f);

            axes[1] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                color: new OpenTK.Mathematics.Vector4(0f, 1f, 0f, 1f)
            );
            axes[1].Transform.Rotation = new OpenTK.Mathematics.Vector3(90f, 0f, 0f);
            sides[1] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(0f, 1f, 0f, 1f)
            );
            sides[1].Transform.Position = new OpenTK.Mathematics.Vector3(1f, 0f, 0f);

            axes[2] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                color: new OpenTK.Mathematics.Vector4(0f, 0f, 1f, 1f)
            );
            axes[2].Transform.Rotation = new OpenTK.Mathematics.Vector3(0f, 0f, 0f);
            sides[2] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(0f, 0f, 1f, 1f)
            );
            sides[2].Transform.Position = new OpenTK.Mathematics.Vector3(0f, 0f, 1f);
        }

        public override void Render()
        {
            axes[0].PrepareRender(MainInstance.light);
            foreach (Cylinder axe in axes)
                axe.Render();
            axes[0].EndRender();

            sides[0].PrepareRender(MainInstance.light);
            foreach (Sphere side in sides)
                side.Render();
            sides[0].EndRender();
        }
    }
}
