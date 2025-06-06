using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class PhongTest : RenderScript
    {
        private Mesh Mesh { get; set; }
        private Vector3 origPos = new();
        public override void Init()
        {
            // create geometry
            Sphere geometry = new Sphere(16, 16, 1f);
            //Cylinder geometry = new Cylinder(3, 16, 2, 1);

            // create material
            PhongMaterial material = PhongMaterial.Pearl;

            // create mesh
            this.Mesh = new Mesh(geometry, material, name: "Circle movement sphere");

            // add our mesh to the scene
            Scene.Add(this.Mesh);

            // change the position
            this.Mesh.Transform.Position.X += 5;
            this.Mesh.Transform.Position.Y += 5;

            // original position for reference for spinning
            this.origPos = new(this.Mesh.Transform.Position);


            // add the cullfacing
            this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
        }
        public override void Advance()
        {
            float t = (float)Timer.Elapsed.TotalMilliseconds * 0.04f;
            float sin = MathMisc.Sinf(t);
            float cos = MathMisc.Cosf(t);

            this.Mesh.Transform.Position = new Vector3(
                origPos.X * sin,
                origPos.Y * cos,
                origPos.Z
                );

            this.Mesh.Transform.UpdateMatrix();

        }

        public override void OnResourceRefresh()
        {
            this.Mesh.Material = PhongMaterial.Pearl;
        }
    }
}
