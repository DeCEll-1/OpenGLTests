using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Shaders.Compute;
using RGL.API.Rendering.Textures;
using RGL.Classes.API.Rendering.Shaders;
using static RGL.API.JSON.MCSDFJSON;

namespace RGL.API
{
    public static class Resources
    {
        public static Dictionary<string, Texture> Textures { get; set; } = new();
        public static Dictionary<string, Shader> Shaders { get; set; } = new();
        public static Dictionary<string, ComputeShader> CompShaders { get; set; } = new();
        public static Dictionary<string, FontJson> Fonts { get; set; } = new();
        public static Dictionary<string, Cubemap> Cubemaps { get; set; } = new();
        public static Dictionary<string, Geometry3D> Geometries { get; set; } = new();
        public static Dictionary<string, Material> Materials { get; set; } = new();
    }
}
