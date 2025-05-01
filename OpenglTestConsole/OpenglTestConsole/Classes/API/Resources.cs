using OpenglTestConsole.Classes.API.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;

namespace OpenglTestConsole.Classes.API
{
    public static class Resources
    {
        public static Dictionary<string, Texture> Textures { get; set; } = new();
        public static Dictionary<string, Shader> Shaders { get; set; } = new();
        public static Dictionary<string, ComputeShader> CompShaders { get; set; } = new();
        public static Dictionary<string, FontJson> Fonts { get; set; } = new();
    }
}
