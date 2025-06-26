using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;

namespace RGL.API.Rendering.Materials
{
    public class PostProcessingMaterial : Material
    {
        protected PostProcessingMaterial() { }
        public PostProcessingMaterial(Shader shader) { PPShader = shader; }
        public FBO? FBOToReadFrom { get; set; }
        internal Shader PPShader;

        public override Shader Shader => PPShader;

        public override void Apply(Scene scene)
        {// remember that we use tex 0 and 1 for these while making shaders
            Shader.UniformManager.SetTexture("colorBuffer", FBOToReadFrom!.ColorTexture, TextureUnit.Texture0);
            Shader.UniformManager.SetTexture("depthBuffer", FBOToReadFrom.DepthStencilTexture, TextureUnit.Texture1);
        }
    }
}
