using System.Reflection;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Shaders.Compute;
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
            // AddMaterials();
            AddComputeShaders();
            AddFonts();
        }

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
        }

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

        private static void AddShaders()
        {
            foreach (var shader in typeof(ResourcePaths.Materials).GetNestedTypes())
            {
                AddShader(shader);
            }
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
            Resources.Shaders[shaderName].Init();
        }

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
    }
}
