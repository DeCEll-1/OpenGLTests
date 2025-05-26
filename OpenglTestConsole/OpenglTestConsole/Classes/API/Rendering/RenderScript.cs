using System.Diagnostics;
using OpenTK.Windowing.Common;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public abstract class RenderScript
    {
        public FrameEventArgs args;
        public Camera Camera { get; set; }
        public Stopwatch Timer { get; set; }
        public Scene Scene { get; set; }
        public abstract void Init();
        public abstract void Advance();

        public virtual void OnResourceRefresh() { }
        public virtual void BeforeRender() { }
        public virtual void AfterRender() { }
    }
}
