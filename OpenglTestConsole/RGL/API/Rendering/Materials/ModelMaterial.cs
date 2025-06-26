using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    public class ModelMaterial : Material
    {
        public override Shader Shader => Resources.Shaders[RGLResources.Shaders.Model.Name];
        public Texture Color { get; }
        public Texture Occlusion { get; }
        public Texture Normals { get; }

        public ModelMaterial(Texture color, Texture occlusion, Texture normal)
        {
            Color = color;
            Occlusion = occlusion;
            Normals = normal;
        }
        public override void Apply(Scene scene)
        {
            Shader.UniformManager.SetTexture("color", Color, TextureUnit.Texture0);
            Shader.UniformManager.SetTexture("occlusion", Occlusion, TextureUnit.Texture1);
            Shader.UniformManager.SetTexture("normal", Normals, TextureUnit.Texture2);
        }
    }
}
