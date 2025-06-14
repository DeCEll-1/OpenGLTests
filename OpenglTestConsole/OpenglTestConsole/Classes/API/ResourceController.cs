using System.Reflection;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
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

            foreach (KeyValuePair<string, Cubemap> item in Resources.Cubemaps)
                item.Value.Dispose();


            Resources.Shaders.Clear();
            Resources.Textures.Clear();
            Resources.CompShaders.Clear();
            Resources.Fonts.Clear();
            Resources.Cubemaps.Clear();
            Resources.Geometries.Clear();

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
            AddModels();
        }
        #region textures
        private static void AddTextures()
        {
            foreach (FieldInfo texture in typeof(ResourcePaths.Resources.Textures).GetFields())
                AddTexture((string)texture.GetValue(null)!);
        }

        private static Texture AddTexture(string texturePath)
        {
            // add the texture to the resources
            Resources.Textures.Add(texturePath, Texture.LoadFromFile(texturePath));
            Logger.Log($"Loading {LogColors.Green("Texture")} {LogColors.BrightWhite(texturePath)}", LogLevel.Detail);
            Logger.PushIndentLevel();
            Resources.Textures[texturePath].Init();
            Logger.PopIndentLevel();
            return Resources.Textures[texturePath];
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

            Logger.Log($"Loading {LogColors.Green("Compute Shader")} {LogColors.BrightWhite(shaderName)}", LogLevel.Detail);
            Logger.PushIndentLevel();
            Resources.CompShaders[shaderName].Init();
            Logger.PopIndentLevel();
        }
        #endregion
        #region shaders
        private static void AddShaders()
        {
            foreach (var shader in typeof(ResourcePaths.Shaders).GetNestedTypes())
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
                Resources.Shaders.Add(shaderName, new(vertex, fragment, name: shaderName));
            else
                Resources.Shaders.Add(shaderName, new(vertex, fragment, geometry, name: shaderName));

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
            string fontPNGPath = (string)type!.GetField("PNG")!.GetValue(null)!;

            Resources.Fonts.Add(fontName, MCSDFJSON.GetFontJson(fontJSONPath)!);

            AddTexture(fontPNGPath);
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
            Logger.PushIndentLevel();

            Resources.Cubemaps.Add(cubemapName, new(textures));
            Resources.Cubemaps[cubemapName].Init();

            Logger.PopIndentLevel();
        }
        #endregion
        #region models
        private static void AddModels()
        {
            foreach (var map in typeof(ResourcePaths.Geometries.Models).GetNestedTypes())
                AddModel(map);
        }
        private static void AddModel(Type type)
        {
            // get the cubemap information
            string modelName = (string)type!.GetField("Name")!.GetValue(null)!;

            string objPath = (string)type!.GetField("OBJ")!.GetValue(null)!;

            string colorTexturePath = (string)type!.GetField("Color")!.GetValue(null)!;
            string occlusionTexturePath = (string)type!.GetField("Occlusion")!.GetValue(null)!;
            string normalTextureMap = (string)type!.GetField("Normal")!.GetValue(null)!;

            Logger.Log($"Loading {LogColors.G("Model")} {LogColors.BW(modelName)}", LogLevel.Detail);

            Logger.PushIndentLevel();

            Resources.Materials.Add(modelName, new ModelMaterial(
                color: AddTexture(colorTexturePath),
                occlusion: AddTexture(occlusionTexturePath),
                normal: AddTexture(normalTextureMap)
            ));


            Resources.Geometries.Add(modelName, new Model3DGeometry(objPath));

            Logger.PopIndentLevel();

        }

        #endregion
    }
}
