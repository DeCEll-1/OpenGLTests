using System.Reflection;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Shaders.Compute;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;

namespace OpenglTestConsole.Classes.API
{
    public class ResourceController
    {
        public static void Refresh()
        {
            foreach (KeyValuePair<string, Shader> item in Resources.Shaders)
                item.Value.Dispose();

            foreach (KeyValuePair<string, ComputeShader> item in Resources.CompShaders)
                item.Value.Dispose();

            foreach (KeyValuePair<string, Texture> item in Resources.Textures)
                item.Value.Dispose();

            Resources.Shaders.Clear();
            Resources.Textures.Clear();
            Resources.CompShaders.Clear();
            Resources.Fonts.Clear();

            Init();
        }

        public static void Init()
        {
            AddTextures();
            AddShaders();
            // interesting intellisense suggestion aint it!
            // however materials are pmuch shaders, so
            // can do some texture pbr stuff ig
            // AddMaterials();
            AddComputeShaders();
            AddFonts();
            AddCubemaps();
        }
        #region textures
        private static void AddTextures()
        {
            foreach (FieldInfo texture in typeof(ResourcePaths.Resources.Textures).GetFields())
                AddTexture(texture);
        }

        private static void AddTexture(FieldInfo texture)
        {
            // get the constant path value from the class
            string texturePath = (string)texture.GetValue(null)!;
            // add the texture to the resources
            Resources.Textures.Add(texturePath, Texture.LoadFromFile(texturePath));
            Logger.Log($"Loading {LogColors.Green("Texture")} {LogColors.BrightWhite(texturePath)}", LogLevel.Detail);
            Resources.Textures[texturePath].Init();
        }
        #endregion
        #region compute shaders
        private static void AddComputeShaders()
        {
            foreach (Type compShader in typeof(ResourcePaths.ComputeShaders).GetNestedTypes())
            {
                AddComputeShader(compShader);
            }
        }

        private static void AddComputeShader(Type type)
        {
            string shaderName = (string)type!.GetField("Name")!.GetValue(null)!;
            string compute = (string)type!.GetField("Compute")!.GetValue(null)!;
            // add the shader to the resources
            Resources.CompShaders.Add(shaderName, new(compute));
            Resources.CompShaders[shaderName].Init();
        }
        #endregion
        #region shaders
        private static void AddShaders()
        {
            foreach (var shader in typeof(ResourcePaths.Materials).GetNestedTypes())
                AddShader(shader);
        }

        private static void AddShader(Type type)
        {
            // get the shader information
            string shaderName = (string)type!.GetField("Name")!.GetValue(null)!;
            string fragment = (string)type!.GetField("Fragment")!.GetValue(null)!;
            string vertex = (string)type!.GetField("Vertex")!.GetValue(null)!;

            string? geometry = (string?)type?.GetField("Geometry")?.GetValue(null);
            if (string.IsNullOrEmpty(geometry))
                Resources.Shaders.Add(shaderName, new(vertex, fragment));
            else
                Resources.Shaders.Add(shaderName, new(vertex, fragment, geometry));
            // add the shader to the resources
            Logger.Log($"Loading {LogColors.Green("Shader")} {LogColors.BrightWhite(shaderName)}", LogLevel.Detail);

            Resources.Shaders[shaderName].Init();
        }
        #endregion
        #region fonts
        private static void AddFonts()
        {
            foreach (var font in typeof(ResourcePaths.Fonts).GetNestedTypes())
            {
                AddFont(font);
            }
        }

        private static void AddFont(Type type)
        {
            string fontName = (string)type!.GetField("Name")!.GetValue(null)!;
            string fontJSONPath = (string)type!.GetField("JSON")!.GetValue(null)!;
            FieldInfo fontPNGField = type.GetField("PNG")!;
            Resources.Fonts.Add(fontName, MCSDFJSON.GetFontJson(fontJSONPath)!);

            AddTexture(fontPNGField);
        }
        #endregion
        #region cubemaps
        private static void AddCubemaps()
        {
            foreach (var map in typeof(ResourcePaths.Cubemaps).GetNestedTypes())
                AddCubemap(map);
        }
        private static void AddCubemap(Type type)
        {
            // get the cubemap information
            string cubemapName = (string)type!.GetField("Name")!.GetValue(null)!;

            string rightSide = (string)type!.GetField("Right")!.GetValue(null)!;
            string leftSide = (string)type!.GetField("Left")!.GetValue(null)!;
            string topSide = (string)type!.GetField("Top")!.GetValue(null)!;
            string bottomSide = (string)type!.GetField("Bottom")!.GetValue(null)!;
            string backSide = (string)type!.GetField("Back")!.GetValue(null)!;
            string frontSide = (string)type!.GetField("Front")!.GetValue(null)!;


            Texture[] textures =
                [
                    Texture.LoadFromFile(rightSide),
                    Texture.LoadFromFile(leftSide),
                    Texture.LoadFromFile(topSide),
                    Texture.LoadFromFile(bottomSide),
                    Texture.LoadFromFile(backSide),
                    Texture.LoadFromFile(frontSide),
                ];


            Logger.Log($"Loading {LogColors.Green("Cubemap")} {LogColors.BrightWhite(cubemapName)}", LogLevel.Detail);

            Resources.Cubemaps.Add(cubemapName, new(textures));
            Resources.Cubemaps[cubemapName].Init();
        }
        #endregion
    }
}
