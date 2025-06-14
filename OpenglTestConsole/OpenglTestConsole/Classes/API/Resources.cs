﻿using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Shaders.Compute;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;

namespace OpenglTestConsole.Classes.API
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
