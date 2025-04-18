using OpenglTestConsole.Classes.impl.rendering;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Rendering;
using OpenglTestConsole.Classes.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenglTestConsole.Classes.API.Extensions;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class RenderCoordinates : RenderScript
    {
        private Cylinder[] axes = new Cylinder[3];
        private Sphere[] sides = new Sphere[3];
        private Square[] images = new Square[2];
        public override void Init()
        {
            // y -> z
            // x -> y
            // z -> x
            axes[0] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                Shader: ResourcePaths.ShaderNames.objectMonoColor,
                color: new OpenTK.Mathematics.Vector4(1f, 0f, 0f, 1f)
            );
            axes[0].Transform.Rotation = new Vector3(90f, 0f, 0f);
            sides[0] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(1f, 0f, 0f, 1f),
                shader: ResourcePaths.ShaderNames.objectMonoColor
            );
            sides[0].Transform.Position = new Vector3(0f, 0f, 1f);

            axes[1] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                Shader: ResourcePaths.ShaderNames.objectMonoColor,
                color: new OpenTK.Mathematics.Vector4(0f, 1f, 0f, 1f)
            );
            axes[1].Transform.Rotation = new Vector3(0f, 90f, 0f);
            sides[1] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(0f, 1f, 0f, 1f),
                shader: ResourcePaths.ShaderNames.objectMonoColor
            );
            sides[1].Transform.Position = new Vector3(1f, 0f, 0f);

            axes[2] = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                Shader: ResourcePaths.ShaderNames.objectMonoColor,
                color: new OpenTK.Mathematics.Vector4(0f, 0f, 1f, 1f)
            );
            axes[2].Transform.Rotation = new Vector3(0f, 0f, 90f);
            sides[2] = new Sphere(
                camera: this.Camera,
                stackCount: 8,
                sectorCount: 12,
                radius: 0.3f,
                color: new OpenTK.Mathematics.Vector4(0f, 0f, 1f, 1f),
                shader: ResourcePaths.ShaderNames.objectMonoColor
            );
            sides[2].Transform.Position = new Vector3(0f, -1f, 0f);

            Vector3 coordinateSystemSize = new Vector3(2f, 1f, 0f);
            Vector3[] coordinateSystem = coordinateSystemSize.CreateSquare();
            images[0] = new Square(
                camera: this.Camera,
                vectors: coordinateSystem,
                shader: ResourcePaths.ShaderNames.objectTextured,
                texture: Resources.Textures[ResourcePaths.Textures.CoordinateSystem_png]
            );
            images[0].Transform.Position = new Vector3(-2f, 1.5f, 0f);

            Vector3 rotationSystemSize = new Vector3(2.5f, 1.5f, 0f);
            Vector3[] rotationSystem = rotationSystemSize.CreateSquare();
            images[1] = new Square(
                camera: this.Camera,
                vectors: rotationSystem,
                shader: ResourcePaths.ShaderNames.objectTextured,
                texture: Resources.Textures[ResourcePaths.Textures.RotationSystem_png]
            );
            images[1].Transform.Position = new Vector3(2f, 1.5f, 0f);
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

            images[0].PrepareRender(MainInstance.light);
            foreach (Square image in images)
                image.Render();
            images[0].EndRender();
        }
    }
}
