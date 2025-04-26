using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Classes
{
    public class ResourceController
    {
        public static void Init()
        {
            AddTextures();
            AddShaders();
            // interesting intellisense suggestion aint it!
            // AddMaterials();
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
            Resources.Textures.Add(texturePath, new(texturePath));
            // init the texture we setted
            Resources.Textures[texturePath].Init();
        }
        private static void AddShaders()
        {
            int i = 0;
            foreach (var shader in typeof(ResourcePaths.VertexShaders).GetFields())
            {
                AddShader(shader, i);
                i++;
            }
        }
        private static void AddShader(FieldInfo shader, int i)
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
