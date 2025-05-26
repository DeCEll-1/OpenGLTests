using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Extensions;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class RenderLight : RenderScript
    {
        private Mesh Mesh;

        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 0.4f);

            MonoColorMaterial material = new(new(1f));
            //PhongMaterial material = new PhongMaterial(new(1), new(1), new(1), 1);

            this.Mesh = new Mesh(geometry, material);

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
