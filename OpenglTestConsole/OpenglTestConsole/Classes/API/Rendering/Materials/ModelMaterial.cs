using ICSharpCode.Decompiler.CSharp.Syntax;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public class ModelMaterial : Material
    {
        public override Shader Shader => Resources.Shaders[ResourcePaths.Shaders.Model.Name];
        public Texture Color { get; }
        public Texture Occlusion { get; }
        public Texture Normals { get; }

        public ModelMaterial(Texture color, Texture occlusion, Texture normal)
        {
            Color = color;
            Occlusion = occlusion;
            Normals = normal;
        }
        public override void Apply()
        {
            Shader.UniformManager.SetTexture("color", Color, TextureUnit.Texture0);
            Shader.UniformManager.SetTexture("occlusion", Occlusion, TextureUnit.Texture1);
            Shader.UniformManager.SetTexture("normal", Normals, TextureUnit.Texture2);
        }
    }
}
