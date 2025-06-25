using RGL.API;
using RGL.API.Rendering;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Shaders;
using RGL.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.Classes.Implementations.RenderScripts.TestRSs
{
    public class RenderFumo : RenderScript
    {
        public override void Init()
        {
            Geometry3D geometry = Resources.Geometries[AppResources.Geometries.Models.Cirno.Name];
            Material material = Resources.Materials[AppResources.Geometries.Models.Cirno.Name];
            Transform temp = new();
            temp.Rotation.Y = -90;
            temp.UpdateMatrix();
            geometry.ApplyTransformation(temp.GetModelMatrix());

            Mesh mesh = new Mesh(geometry, material, name: "CiRNO HELL YEAH");
            Scene.Add(mesh);

            mesh.Transform.Scale = new(0.1f);
            mesh.Transform.Position.X += 5;
            mesh.Transform.Position.Y += 3;
            mesh.Transform.Rotation.Z += 90;
            mesh.Transform.UpdateMatrix();

            mesh.CapsToEnable.Add(EnableCap.CullFace);

        }

        public override void Advance() { }

    }
}
