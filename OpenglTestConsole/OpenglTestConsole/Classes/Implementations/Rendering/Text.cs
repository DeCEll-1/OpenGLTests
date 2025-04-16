using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    public class Text : Mesh
    {
        public string TextString { get; set; } = "Hello World!";
        public FontJson Font { get; set; }
        [SetsRequiredMembers]
        public Text(Camera camera, string shader = "MCSDF") : base(camera, shader:shader)
        {

        }
    }
}
