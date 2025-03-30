using OpenTK.Windowing.Desktop;

namespace OpenglTestConsole.classes
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logger.Log($"Started app with arguments:\n{string.Concat(args)}", LogLevel.Info);
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "OpenGL Test Console"
            };

            nativeWindowSettings.Vsync = OpenTK.Windowing.Common.VSyncMode.On;

            Main main = new Main(gameWindowSettings, nativeWindowSettings);
            main.Run();
            
        }
    }
}
