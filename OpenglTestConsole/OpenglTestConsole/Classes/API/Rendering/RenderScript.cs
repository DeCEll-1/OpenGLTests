using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Windowing.Common;
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
        public FrameEventArgs args;
        public Camera Camera { get; set; }
        public Stopwatch Timer { get; set; }
        public Scene Scene { get; set; }
        public abstract void Init();
        public abstract void Render();
    }
}
