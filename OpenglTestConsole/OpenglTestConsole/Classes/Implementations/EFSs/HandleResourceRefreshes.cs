using OpenglTestConsole.Classes.API;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenglTestConsole.Classes.Implementations.EFSs
{
    public class HandleResourceRefreshes : EveryFrameScript
    {
        public override void Init() { }

        public override void Advance()
        {
            if (!KeyboardState.IsKeyPressed(Keys.F8))
                return;
            ResourceController.Refresh();

            foreach (var script in MainInstance.RenderScripts)
                script.OnResourceRefresh();
        }
    }
}
