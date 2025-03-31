using OpenglTestConsole.classes.impl.rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Classes;
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
        private List<StarscapeSystemData> SystemsData { get; set; } = new List<StarscapeSystemData>();
        private List<Sphere> spheres { get; set; } = new List<Sphere>();
        public override void Init()
        {
            this.SystemsData = LoadJsonFromFile<List<StarscapeSystemData>>.Load("Resources/map.json");

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
                        stackCount: 2,
                        sectorCount: 3,
                        radius: 1f,
                        color: color
                    );

                sector.Transform.Position = data.Position / 50f;

                spheres.Add(sector);
            }

            this.SystemsData = new();
        }

        public override void Render()
        {
            foreach (var sphere in spheres)
            {
                sphere.Render(Main.light);
            }
        }
    }
}
