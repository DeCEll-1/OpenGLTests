using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenTK.Mathematics;
using OpenglTestConsole.Generated.Paths;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public class TextureMaterial : Material
    {
        public Texture Texture { get; set; }
        public Vector4 Color { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        public TextureMaterial() { this.Shader = Resources.Shaders[ResourcePaths.Materials.Texture.Name]; }
        public TextureMaterial(Texture texture, Vector4? color = null)
        {
            this.Texture = texture; this.Color = color ?? new Vector4(1f, 1f, 1f, 1f);
            this.Shader = Resources.Shaders[ResourcePaths.Materials.Texture.Name];
        }
        public override void Apply()
        {
            Shader.UniformManager.SetTexture("material.texture", Texture, TextureUnit.Texture0);
            Shader.UniformManager.SetVector4("material.colMultiplier", Color);
        }
    }
}
