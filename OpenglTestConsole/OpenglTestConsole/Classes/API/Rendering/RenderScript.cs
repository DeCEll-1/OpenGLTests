using OpenglTestConsole.classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
