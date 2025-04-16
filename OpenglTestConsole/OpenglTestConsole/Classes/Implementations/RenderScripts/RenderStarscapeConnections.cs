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
        public override void Init()
        {
            List<StarscapeSystemConnectionData> ConnectionData = LoadJsonFromFile<List<StarscapeSystemConnectionData>>.Load(ResourcePaths.StarscapeMapDatas.cylinders_json)!;
            foreach (StarscapeSystemConnectionData connection in ConnectionData)
            {

                Cylinder cylinder = new Cylinder(
                 camera: this.Camera,
                 StackCount: 1,
                 SectorCount: 3,
                 Radius: 0.1f,
                 Height: connection.Height * scale ,
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

            CylinderInstanceRenderer.FinishAddingElemets();

           Vector4[] colors = CylinderInstanceRenderer.GetFieldValuesFromMeshes<Vector4>("Color");

            CylinderInstanceRenderer.SetVector4(colors, 2);

        }

        public override void Render()
        {

            CylinderInstanceRenderer.PrepareRender(MainInstance.light);
            CylinderInstanceRenderer.RenderWithIndices();
            CylinderInstanceRenderer.EndRender();

        }
    }
}
