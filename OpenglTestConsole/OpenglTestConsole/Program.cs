﻿global using OpenTK.Graphics.OpenGL;
using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.misc;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenTK.Windowing.Desktop;
namespace OpenglTestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logger.Log($"Started app with arguments:\n{string.Concat(args)}", LogLevel.Info);


            Settings.Fov = 90f;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 800),
                Title = "OpenGL Test Console",
                DepthBits = 24,
            };

            gameWindowSettings.UpdateFrequency = 60;

            Main main = new Main(gameWindowSettings, nativeWindowSettings);
            main.Run();

        }
    }
}
