using OpenglTestConsole.Classes.API.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    internal class StandartMaterial : Material
    {
        public bool Wireframe { get; set; }
        public bool FlatShaded { get; set; }

        public StandartMaterial() { }
        public override void Apply()
        {
            // add shader sets here

        }
    }
}
