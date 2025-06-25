global using OpenTK.Graphics.OpenGL;
global using OpenglTestConsole.Generated.Paths;
global using RGL.Generated.Paths;
using RGL.Classes;
using OpenTK.Windowing.Desktop;
using RGL.API;
using RGL.API.Misc;

namespace RGL
{
    public class Program
    {
        public static Main main;
        static void Main(string[] args)
        {
            Logger.Log($"Started app with arguments:\n{string.Concat(args)}", LogLevel.Info);

            APISettings.Fov = 90f;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = APISettings.Resolution,
                Title = "OpenGL Test Console",
                DepthBits = 24,
            };

            gameWindowSettings.UpdateFrequency = 60;

            main = new Main(gameWindowSettings, nativeWindowSettings);
            main.Run();
        }
    }
}
