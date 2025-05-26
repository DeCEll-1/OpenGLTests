using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Shaders.Compute;
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
