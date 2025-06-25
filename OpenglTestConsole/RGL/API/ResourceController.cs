using RGL.API.JSON;
using RGL.API.Misc;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.Shaders.Compute;
using RGL.API.Rendering.Textures;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;
using System.Reflection;
using static RGL.Generated.Paths.RGLResources.Geometries;

namespace RGL.API
{
    public class ResourceController
    {
        private static Type AppResources;
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

        public static void Init(Type appResources = null)
        {
            if (appResources != null)
                AppResources = appResources;

            AddTextures();
            AddShaders();
            AddComputeShaders();
            AddFonts();
            AddCubemaps();
            AddModels();
        }
        #region textures
        private static void AddTextures()
        {
            foreach (FieldInfo texture in typeof(RGLResources.Textures).GetFields())
                AddTexture((string)texture.GetValue(null)!);

            Type? appTextures = AppResources.GetNestedType("Textures");
            if (appTextures == null)
                return;

            foreach (FieldInfo texture in appTextures.GetFields())
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
            foreach (Type compShader in typeof(RGLResources.ComputeShaders).GetNestedTypes())
                AddComputeShader(compShader);

            Type? appCompShaders = AppResources.GetNestedType("ComputeShaders");
            if (appCompShaders == null)
                return;

            foreach (var map in appCompShaders.GetNestedTypes())
                AddComputeShader(map);
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
            foreach (var shader in typeof(RGLResources.Shaders).GetNestedTypes())
                AddShader(shader);

            Type? appShaders = AppResources.GetNestedType("Shaders");
            if (appShaders == null)
                return;

            foreach (var shader in appShaders.GetNestedTypes())
                AddShader(shader);
        }


        private static void AddShader(Type type)
        {
            string shaderName = (string)type.GetField("Name")!.GetValue(null)!;

            string fragmentPath = (string)type.GetField("Fragment")!.GetValue(null)!;
            string? fragmentContent = type.GetField("FragmentFileContent")?.GetValue(null) as string;

            string vertexPath = (string)type.GetField("Vertex")!.GetValue(null)!;
            string? vertexContent = type.GetField("VertexFileContent")?.GetValue(null) as string;

            string? geometryPath = type.GetField("Geometry")?.GetValue(null) as string;
            string? geometryContent = type.GetField("GeometryFileContent")?.GetValue(null) as string;

            Shader shader;
            if (string.IsNullOrEmpty(geometryContent) && string.IsNullOrEmpty(geometryPath))
            {
                shader = new Shader(
                    vertexSource: vertexContent ?? File.ReadAllText(vertexPath),
                    fragmentSource: fragmentContent ?? File.ReadAllText(fragmentPath),
                    name: shaderName
                );
            }
            else
            {
                shader = new Shader(
                    vertexSource: vertexContent ?? File.ReadAllText(vertexPath),
                    fragmentSource: fragmentContent ?? File.ReadAllText(fragmentPath),
                    geometrySource: geometryContent! ?? File.ReadAllText(geometryPath!),
                    name: shaderName
                );
            }

            // Add to resources and initialize
            Resources.Shaders.Add(shaderName, shader);

            Logger.Log($"Loading {LogColors.Green("Shader")} {LogColors.BrightWhite(shaderName)}", LogLevel.Detail);
            shader.Init();
        }

        #endregion
        #region fonts
        private static void AddFonts()
        {
            foreach (var font in typeof(RGLResources.Fonts).GetNestedTypes())
                AddFont(font);

            Type? appFonts = AppResources.GetNestedType("Fonts");
            if (appFonts == null)
                return;

            foreach (var font in appFonts.GetNestedTypes())
                AddFont(font);
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
            foreach (var map in typeof(RGLResources.Cubemaps).GetNestedTypes())
                AddCubemap(map);

            Type? appCubemaps = AppResources.GetNestedType("Cubemaps");
            if (appCubemaps == null)
                return;

            foreach (var map in appCubemaps.GetNestedTypes())
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
            foreach (var map in typeof(Models).GetNestedTypes())
                AddModel(map);

            Type? appModels = AppResources.GetNestedType("Geometries")?.GetNestedType("Models");

            if (appModels == null)
                return;

            foreach (var map in appModels.GetNestedTypes())
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
