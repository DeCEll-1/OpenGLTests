using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class SquareTest : RenderScript
    {
        private Mesh Mesh { get; set; }

        public override void Init()
        {
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(1, 3, 3f, 1f);

            //Cube geometry = new Cube(new(1));
            //Square geometry = new Square(new(1));

            PhongMaterial material = PhongMaterial.Pearl;

            this.Mesh = new Mesh(geometry, material);

            Scene.Add(this.Mesh);
            this.Mesh.Transform.Position.X += 10;
            this.Mesh.Transform.UpdateMatrix();

            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }
        public override void Advance()
        {
            float t = (float)Timer.Elapsed.TotalMilliseconds * 0.04f;
            float sin = MathMisc.Sinf(t) * 5f;

            this.Mesh.Transform.Position.X = sin;
            this.Mesh.Transform.UpdateMatrix();
        }

        public override void OnResourceRefresh()
        {
            Mesh.Material = PhongMaterial.Pearl;
        }
    }
}
