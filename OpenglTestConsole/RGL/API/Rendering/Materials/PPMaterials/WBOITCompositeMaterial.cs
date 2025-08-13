using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.Rendering.Materials.PPMaterials
{
    public class WBOITCompositeMaterial : PostProcessingMaterial
    {
        public FBO WBOITFBO { get; set; }
        public WBOITCompositeMaterial(FBO WBOITFBO) { PPShader = Resources.Shaders[RGLResources.Shaders.PPWBOITCompositePass.Name].Opaque; this.WBOITFBO = WBOITFBO; }

        public override void Apply(Scene scene)
        {
            // we dont really need the color and depth buffer 
            base.Apply(scene);

            Shader.UniformManager.SetTexture("uAccumTex", WBOITFBO.ColorTextures[0], TextureUnit.Texture2);
            Shader.UniformManager.SetTexture("uRevealTex", WBOITFBO.ColorTextures[1], TextureUnit.Texture3);
        }

    }
}
