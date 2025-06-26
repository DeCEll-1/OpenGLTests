using OpenTK.Mathematics;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    public class PPFogMaterial : PostProcessingMaterial
    {
        public PPFogMaterial()
        {
            PPShader = Resources.Shaders[RGLResources.Shaders.PPDepthDisplay.Name];
        }

        public override void Apply(Scene scene)
        {
            base.Apply(scene);

            Shader.UniformManager.SetColor3("fogColor", Color4.BlueViolet);

            Shader.UniformManager.SetFloat("near", scene.Camera.depthNear);
            Shader.UniformManager.SetFloat("far", scene.Camera.depthFar);

            Shader.UniformManager.SetFloat("fogDensity", 0.1f);
        }
    }
}
