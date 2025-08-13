using OpenTK.Mathematics;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;

namespace RGL.API.Rendering.Materials.PPMaterials
{
    public class PPFogMaterial : PostProcessingMaterial
    {
        public float fogDensity { get; set; }
        public float fogStart { get; set; }
        public Color4 fogColor { get; set; }

        public PPFogMaterial(float fogDensity = 0.1f,float fogStart = 0f, Color4? fogColor = null)
        {
            PPShader = Resources.Shaders[RGLResources.Shaders.PPDepthDisplay.Name].Opaque;
            this.fogStart = fogStart;
            this.fogColor = fogColor ?? Color4.White;
            this.fogDensity = fogDensity;
        }


        public override void Apply(Scene scene)
        {
            base.Apply(scene);

            Shader.UniformManager.SetColor3("fogColor", fogColor);

            Shader.UniformManager.SetFloat("near", scene.Camera.depthNear);
            Shader.UniformManager.SetFloat("far", scene.Camera.depthFar);


            Shader.UniformManager.SetFloat("fogStart", fogStart);


            Shader.UniformManager.SetFloat("fogDensity", fogDensity);
        }
    }
}
