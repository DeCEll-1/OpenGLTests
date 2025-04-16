using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes
{
    public abstract class EveryFrameScript
    {
        public MouseState MouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public Camera Camera { get; set; }
        public Main Main { get; set; }
        public FrameEventArgs args { get; set; }
        public abstract void Init();
        public abstract void Advance();

    }
}
