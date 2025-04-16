using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public abstract class RenderScript
    {
        public Camera Camera { get; set; }
        public Stopwatch Timer { get; set; }
        public Main MainInstance { get; set; }
        public abstract void Init();
        public abstract void Render();
    }
}
