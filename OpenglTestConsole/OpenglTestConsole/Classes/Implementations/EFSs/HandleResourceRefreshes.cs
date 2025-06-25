using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API;

namespace RGL.Classes.Implementations.EFSs
{
    public class HandleResourceRefreshes : EveryFrameScript
    {
        public override void Init() { }

        public override void Advance()
        {
            if (!KeyboardState.IsKeyPressed(Keys.F8))
                return;
            ResourceController.Refresh();

            foreach (var script in ((Main)MainInstance).RenderScripts)
                script.OnResourceRefresh();
        }
    }
}
