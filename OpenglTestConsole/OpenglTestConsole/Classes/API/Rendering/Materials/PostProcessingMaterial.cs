using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public class PostProcessingMaterial : Material
    {
        public PostProcessingMaterial(Shader shader) { this.Shader = shader; }
        public FBO FBOToReadFrom { get; set; }
        public override void Apply()
        {
            Shader.UniformManager.SetTexture("screen", FBOToReadFrom.ColorTexture, TextureUnit.Texture0);
        }
    }
}
