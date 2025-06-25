using RGL.API.Rendering.Textures;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using GLGLF = OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API;
using RGL.API.Misc;

namespace RGL.Classes.Implementations.EFSs
{
    public class HandlePrintScreen : EveryFrameScript
    {
        public override void Init() { }

        public override void Advance()
        {
            if (!KeyboardState.IsKeyPressed(GLGLF.Keys.F12))
                return;

            DateTime currTime = DateTime.Now;
            string folder = Paths.ScreenshotLocation.ToString();
            string file =
                "Y"
                + currTime.Year
                + "-M"
                + currTime.Month
                + "-D"
                + currTime.Day
                + "-H"
                + currTime.Hour
                + "-M"
                + currTime.Minute
                + "-S"
                + currTime.Second
                + ".jpg";

            Texture tex = RenderMisc.GetSceneTexture(Main.mainScene);

            tex.Init(name: "Print Screen");
            tex.SaveToFile(folder + file);
            //tex.logDisposal = false;
            tex.Dispose();

            Logger.Log(
                $"Saved screenshot to {LogColors.BrightWhite(folder)} as {LogColors.BrightWhite(file)}",
                LogLevel.Detail
            );
        }
    }
}
