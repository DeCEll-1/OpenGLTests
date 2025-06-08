using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class RenderFumo : RenderScript
    {
        public override void Init()
        {
            Geometry3D geometry = Resources.Geometries[ResourcePaths.Geometries.Models.Cirno.Name];
            Material material = Resources.Materials[ResourcePaths.Geometries.Models.Cirno.Name];
            Transform temp = new();
            temp.Rotation.Y = -90;
            temp.UpdateMatrix();
            geometry.ApplyTransformation(temp.GetModelMatrix());

            Mesh mesh = new Mesh(geometry, material, name: "CiRNO HELL YEAH");
            Scene.Add(mesh);

            mesh.Transform.Position.X += 5;
            mesh.Transform.Position.Y += 5;
            mesh.Transform.UpdateMatrix();

        }

        public override void Advance() { }

    }
}
