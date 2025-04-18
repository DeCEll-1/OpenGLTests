using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;
using OpenglTestConsole.Classes.Implementations.Rendering;
using OpenglTestConsole.Classes.API.Extensions;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class TextRendering : RenderScript
    {
        private Text text;
        public override void Init()
        {
            text = new Text(
                camera: Camera,
                fontJson: ResourcePaths.Fonts.ComicSans_json,
                fontTexture: ResourcePaths.Fonts.ComicSans_png,
                text: "Hello World",
                scale: 1f
            );
            text.Transform.Position = new Vector3(3f, 3f, 2f);
            text.ForeColor = VectorExtensions.FromHex("DF85FFFF");
            text.BackColor = VectorExtensions.FromHex("041506FF");

        }

        public override void Render()
        {
            fpsTimer += (float)args.Time;
            float updateFrequency = 2f;
            frameCount++;

            if (fpsTimer >= 1f / updateFrequency)
            {
                text.TextString = $"FPS: {frameCount* updateFrequency}";
                fpsTimer = frameCount = 0;
            }

            text.Render();
        }
        float fpsTimer = 0f; int frameCount = 0;
    }
}
