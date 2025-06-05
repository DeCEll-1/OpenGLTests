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
        protected PostProcessingMaterial() { }
        public PostProcessingMaterial(Shader shader) { this.Shader = shader; }
        public FBO? FBOToReadFrom { get; set; }
        public override void Apply()
        {// remember that we use tex 0 and 1 for these while making shaders
            Shader.UniformManager.SetTexture("colorBuffer", FBOToReadFrom!.ColorTexture, TextureUnit.Texture0);
            Shader.UniformManager.SetTexture("depthBuffer", FBOToReadFrom.DepthStencilTexture, TextureUnit.Texture1);
        }
    }
}
