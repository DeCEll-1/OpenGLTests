using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API.Rendering;

namespace RGL.API
{
    public abstract class EveryFrameScript
    {
        public MouseState MouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public Camera Camera { get; set; }
        public GameWindow MainInstance { get; set; }
        public FrameEventArgs args { get; set; }
        public abstract void Init();
        public abstract void Advance();

        public virtual void OnResourceRefresh() { }
    }
}
