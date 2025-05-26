using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenglTestConsole.Classes.API
{
    public abstract class EveryFrameScript
    {
        public MouseState MouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public Camera Camera { get; set; }
        public Main MainInstance { get; set; }
        public FrameEventArgs args { get; set; }
        public abstract void Init();
        public abstract void Advance();

        public virtual void OnResourceRefresh() { }
    }
}
