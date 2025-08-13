using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    internal class SkyboxMaterial : Material
    {
        private Cubemap cubemap;
        public SkyboxMaterial(Cubemap cubemap)
        {
            this.cubemap = cubemap;
        }

        public override Shader Shader => Resources.Shaders[RGLResources.Shaders.Skybox.Name].Opaque;


        public override void Apply(Scene scene)
        { // set shader values here
            Shader.UniformManager.SetCubemap("skybox", cubemap, TextureUnit.Texture0);
        }
    }
}
