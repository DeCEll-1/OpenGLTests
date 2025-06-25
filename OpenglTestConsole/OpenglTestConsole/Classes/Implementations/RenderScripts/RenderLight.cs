using RGL.API.SceneFolder;
using OpenTK.Mathematics;
using RGL.API.Rendering;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Extensions;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.Geometries;

namespace RGL.Classes.Implementations.RenderScripts
{
    internal class RenderLight : RenderScript
    {
        private Mesh Mesh;

        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 0.4f);

            MonoColorMaterial material = new(new(1f));
            //PhongMaterial material = new PhongMaterial(new(1), new(1), new(1), 1);

            this.Mesh = new Mesh(geometry, material, name: "Light");

            Scene.Add(this.Mesh);

            this.Mesh.Transform.Position = Scene.Lights[0].Position;
            this.Mesh.Transform.UpdateMatrix();


            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }

        public override void Advance()
        {
        }

        public override void OnResourceRefresh()
        {
            this.Mesh.Material = new MonoColorMaterial(Color4.Silver.ToVector4());
        }
    }
}
