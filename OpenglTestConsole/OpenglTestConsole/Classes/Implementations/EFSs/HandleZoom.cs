using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API;
using RGL.API.Misc;

namespace RGL.Classes.impl.EFSs
{
    public class HandleZoom : EveryFrameScript
    {
        private float zoomPercent = 0f;
        private const float zoomSpeedMult = 1.5f;

        private float mainSensitivity = 0.1f;
        private float sensitivityChangeAmount = 0.05f;

        private float mainFov = 90f;
        private const float zoomAmount = 45f;

        public override void Init()
        {
            this.mainSensitivity = APISettings.MouseSensitivity;
            this.mainFov = APISettings.Fov;
        }

        public override void Advance()
        {
            if (KeyboardState.IsKeyDown(Keys.X))
            {
                if (zoomPercent <= 1.0f)
                {
                    zoomPercent += (float)args.Time * zoomSpeedMult;
                    UpdateView();
                }
            }
            else
            {
                if (zoomPercent >= 0.0f)
                {
                    zoomPercent -= (float)args.Time * zoomSpeedMult;
                    UpdateView();
                }
            }
        }

        private void UpdateView()
        {
            APISettings.Fov = mainFov - (EasingFunctions.InOutCubic(zoomPercent) * zoomAmount);
            APISettings.MouseSensitivity =
                mainSensitivity
                - (EasingFunctions.InOutCubic(zoomPercent) * sensitivityChangeAmount);
        }
    }
}
