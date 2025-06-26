using OpenTK.Mathematics;
using RGL.API.Rendering.Shaders;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    public class MonoColorMaterial : Material
    {
        public Vector4 color = new(1);
        public override Shader Shader => Resources.Shaders[RGLResources.Shaders.MonoColor.Name];
        public MonoColorMaterial(Vector4 col)
        {
            color = col;
        }


        public override void Apply(Scene scene)
        {
            Shader.UniformManager.SetVector4("color", color);
        }
    }
}
