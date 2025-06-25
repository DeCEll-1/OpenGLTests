using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials
{
    public class PPGammaCorrection : PostProcessingMaterial
    {
        public PPGammaCorrection() { PPShader = Resources.Shaders[RGLResources.Shaders.PPGammaCorrection.Name]; }

        public override void Apply()
        {
            base.Apply();
            Shader.UniformManager.SetFloat("gamma", APISettings.Gamma);
        }


    }
}
