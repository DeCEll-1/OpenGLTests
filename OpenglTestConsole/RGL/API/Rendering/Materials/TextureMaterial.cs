using OpenTK.Mathematics;
using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    public class TextureMaterial : Material
    {
        public Texture Texture { get; set; }
        public Vector4 Color { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        public override Shader Shader => Resources.Shaders[RGLResources.Shaders.Texture.Name];

        public TextureMaterial(Texture texture, Vector4? color = null)
        {
            Texture = texture; Color = color ?? new Vector4(1f, 1f, 1f, 1f);
        }
        public override void Apply(Scene scene)
        {
            Shader.UniformManager.SetTexture("material.texture", Texture, TextureUnit.Texture0);
            Shader.UniformManager.SetVector4("material.colMultiplier", Color);
        }
    }
}
