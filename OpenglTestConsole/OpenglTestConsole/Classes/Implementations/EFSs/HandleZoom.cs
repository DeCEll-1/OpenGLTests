using OpenglTestConsole.classes.api.misc;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes.impl.EFSs
{
    public class HandleZoom : EveryFrameScript
    {
        private float zoomPercent = 0f;
        private const float mainFov = 90f;
        private const float zoomAmount = 45f;
        private const float zoomSpeedMult = 1.5f;
        public override void Advance()
        {

            if (KeyboardState.IsKeyDown(Keys.X))
            {
                if (zoomPercent<=1.0f)
                {
                    zoomPercent += (float)args.Time * zoomSpeedMult;
                    Camera.Fov = mainFov - (EasingFunctions.InOutCubic(zoomPercent) * zoomAmount);
                }
            }
            else
            {
                if (zoomPercent >= 0.0f)
                {
                    zoomPercent -= (float)args.Time * zoomSpeedMult;
                    Camera.Fov = mainFov - (EasingFunctions.InOutCubic(zoomPercent) * zoomAmount);
                }
            }

        }
    }
}
