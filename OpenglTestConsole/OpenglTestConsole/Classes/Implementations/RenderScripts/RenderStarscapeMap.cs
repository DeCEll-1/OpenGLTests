using OpenglTestConsole.classes.impl.rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    public class RenderStarscapeMap : RenderScript
    {
        public RenderStarscapeMap(float scale) { this.scale = scale; }
        private float scale;
        private List<Sphere> Spheres { get; set; } = new List<Sphere>();
        public override void Init()
        {
            List<StarscapeSystemData> SystemsData = LoadJsonFromFile<List<StarscapeSystemData>>.Load("Resources/map.json");

            foreach (var data in SystemsData)
            {
                Vector4 color = Vector4.Zero;
                Color4 tempCol = Color4.White;
                switch (data.Security)
                {
                    case Security.Wild:
                        tempCol = Color4.Purple;
                        color = new Vector4(tempCol.R, tempCol.G, tempCol.B, tempCol.A);
                        break;
                    case Security.Unsecure:
                        tempCol = Color4.Red;
                        color = new Vector4(tempCol.R, tempCol.G, tempCol.B, tempCol.A);
                        break;
                    case Security.Secure:
                        tempCol = Color4.Blue;
                        color = new Vector4(tempCol.R, tempCol.G, tempCol.B, tempCol.A);
                        break;
                    case Security.Contested:
                        tempCol = Color4.Orange;
                        color = new Vector4(tempCol.R, tempCol.G, tempCol.B, tempCol.A);
                        break;
                    case Security.Core:
                        tempCol = Color4.Green;
                        color = new Vector4(tempCol.R, tempCol.G, tempCol.B, tempCol.A);
                        break;
                    default:
                        break;
                }

                Sphere sector = new(
                        camera: this.Camera,
                        stackCount: 8,
                        sectorCount: 12,
                        radius: 0.5f,
                        color: color
                    );

                Vector3 realPos = data.Position * scale;

                // Create the transformation matrix
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

                sector.Transform.Position = realPos * mirrorMatrix * rotationMatrix * otherMatrix;

                Spheres.Add(sector);
            }

        }

        public override void Render()
        {

            Spheres[0].PrepareRender(MainInstance.light);
            foreach (var sphere in Spheres)
            {
                sphere.Render();
            }
            Spheres[0].EndRender();
        }
    }
}
