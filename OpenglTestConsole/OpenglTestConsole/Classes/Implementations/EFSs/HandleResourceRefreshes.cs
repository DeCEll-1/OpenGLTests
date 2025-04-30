using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.EFSs
{
    public class HandleResourceRefreshes : EveryFrameScript
    {
        public override void Init()
        {
        }

        public override void Advance()
        {
            if (!KeyboardState.IsKeyPressed(Keys.F8)) return;
            ResourceController.Refresh();

            foreach (var script in Main.RenderScripts)
                script.OnResourceRefresh();

        }


    }
}
