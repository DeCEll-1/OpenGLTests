global using OpenTK.Graphics.OpenGL;
using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenTK.Windowing.Desktop;

namespace OpenglTestConsole
{
    public class Program
    {
        public static Main main;
        static void Main(string[] args)
        {
            Logger.Log($"Started app with arguments:\n{string.Concat(args)}", LogLevel.Info);

            Settings.Fov = 90f;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = Settings.Resolution,
                Title = "OpenGL Test Console",
                DepthBits = 24,
            };

            gameWindowSettings.UpdateFrequency = 60;

            main = new Main(gameWindowSettings, nativeWindowSettings);
            main.Run();
        }
    }
}
