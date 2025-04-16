using OpenglTestConsole.Classes.impl.rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenglTestConsole.Classes.Implementations.Rendering;
using OpenTK.Mathematics;
using OpenglTestConsole.Classes.API.Extensions;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using OpenglTestConsole.Classes.Paths;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    public class RenderStarscapeConnections : RenderScript
    {
        public RenderStarscapeConnections(float scale) { this.scale = scale; }
        private float scale;
        private InstancedMesh<Cylinder> CylinderInstanceRenderer = new();
        private InstancedMesh<Square> HELP2 = new();
        private Square HELP3;
        public override void Init()
        {
            List<StarscapeSystemConnectionData> ConnectionData = LoadJsonFromFile<List<StarscapeSystemConnectionData>>.Load(ResourcePaths.StarscapeMapDatas.cylinders_json)!;
            foreach (StarscapeSystemConnectionData connection in ConnectionData.Take(0))
            {

                Cylinder cylinder = new Cylinder(
                 camera: this.Camera,
                 StackCount: 1,
                 SectorCount: 3,
                 Radius: 0.1f,
                 Height: connection.Height * scale,
                 Shader: ResourcePaths.ShaderNames.instancedRenderingMonoColor,
                 color: new Vector4(1f, 1f, 1f, 1f)
                );

                Matrix3 mirrorMatrix = new Matrix3(
                    -1, 0, 0,   // First row
                    0, 1, 0,    // Second row
                    0, 0, 1    // Third row
                );

                Matrix3 rotationMatrix = new Matrix3(
                    0, 1, 0,   // First row
                    -1, 0, 0,    // Second row
                    0, 0, 1    // Third row
                );

                Matrix3 otherMatrix = new Matrix3(
                    1, 0, 0,   // First row
                    0, 0, 1,    // Second row
                    0, -1, 0    // Third row
                );

                Vector3 scaledPos = connection.Center * scale;

                cylinder.Transform.Position = scaledPos * mirrorMatrix * rotationMatrix * otherMatrix;

                Vector3 transformedDirection = connection.Direction * mirrorMatrix * rotationMatrix * otherMatrix;

                transformedDirection = transformedDirection.TurnToEulerRadians().ToDegrees();

                cylinder.Transform.Rotation = transformedDirection;

                // use direction vectors to set the rotation

                CylinderInstanceRenderer.Meshes.Add(cylinder);
            }

            var HELP = new Cylinder(
                camera: this.Camera,
                StackCount: 1,
                SectorCount: 3,
                Radius: 0.1f,
                Height: 5f,
                Shader: ResourcePaths.ShaderNames.instancedRenderingMonoColor,
                color: new OpenTK.Mathematics.Vector4(1f, 0.5f, 0f, 1f)
            );

            CylinderInstanceRenderer.Meshes.Add(HELP);

            CylinderInstanceRenderer.FinishAddingElemets();

            Vector3 square = new Vector3(2f, 2f, 0f);

            HELP2.Meshes.Add(new Square(
                camera: this.Camera,
                vectors: square.CreateSquare(),
                shader: ResourcePaths.ShaderNames.instancedRenderingMonoColor,
                color: new Vector4(1f, 0.5f, 0f, 1f)
            ));

            HELP2.FinishAddingElemets();

            HELP3 = new Square(
                camera: this.Camera,
                vectors: square.CreateSquare(),
                shader: ResourcePaths.ShaderNames.objectMonoColor,
                color: new Vector4(1f, 0.5f, 0f, 1f)
            );

            HELP3.Transform.Position = new(1f, 1f, 1f);

        }

        public override void Render()
        {
            HELP2.PrepareRender(MainInstance.light);
            HELP2.RenderWithIndices();
            HELP2.EndRender();

            HELP3.PrepareRender(MainInstance.light);
            HELP3.Render();
            HELP3.EndRender();

        }
    }
}
