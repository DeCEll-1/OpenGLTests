using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes
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
            foreach (FieldInfo texture in typeof(ResourcePaths.Textures).GetFields())
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
            int i = 0;
            foreach (FieldInfo compShader in typeof(ResourcePaths.ComputeShaders).GetFields())
            {
                AddComputeShader(i);
                i++;
            }
        }
        private static void AddComputeShader(int i)
        {
            // get the shader information
            string shaderName = (string)typeof(ResourcePaths.CompShaderNames).GetFields()[i].GetValue(null)!;
            string compShaderPath = (string)typeof(ResourcePaths.ComputeShaders).GetFields()[i].GetValue(null)!;
            // add the shader to the resources
            Resources.CompShaders.Add(shaderName, new(compShaderPath));
            Resources.CompShaders[shaderName].Init();
        }
        private static void AddShaders()
        {
            int i = 0;
            foreach (var shader in typeof(ResourcePaths.VertexShaders).GetFields())
            {
                AddShader(i);
                i++;
            }
        }
        private static void AddShader(int i)
        {
            // get the shader information
            string shaderName = (string)typeof(ResourcePaths.ShaderNames).GetFields()[i].GetValue(null)!;
            string vertexShaderPath = (string)typeof(ResourcePaths.VertexShaders).GetFields()[i].GetValue(null)!;
            string fragmentShaderPath = (string)typeof(ResourcePaths.FragmentShaders).GetFields()[i].GetValue(null)!;
            // add the shader to the resources
            Resources.Shaders.Add(shaderName, new(vertexShaderPath, fragmentShaderPath));
            Resources.Shaders[shaderName].Init();
        }
        private static void AddFonts()
        {
            foreach (FieldInfo fontField in typeof(ResourcePaths.Fonts).GetFields())
            {
                // get the constant value from the class
                string fontPath = (string)fontField.GetValue(null)!;
                if (fontPath.Contains("_json"))// add the json part of the font
                    AddFont(fontField);

                if (fontPath.Contains("_png"))// add the png part of the font
                    AddTexture(fontField);
            }
        }
        private static void AddFont(FieldInfo fontField)
        {
            string fontPath = (string)fontField.GetValue(null)!;
            Resources.Fonts.Add(fontPath, MCSDFJSON.GetFontJson(fontPath)!);
        }
    }
}
