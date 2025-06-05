using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.SceneFolder;
using OpenglTestConsole.Generated.Paths;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public class PPFogMaterial : PostProcessingMaterial
    {
        public PPFogMaterial()
        {
            this.Shader = Resources.Shaders[ResourcePaths.Materials.PPDepthDisplay.Name];
        }

        public override void Apply()
        {
            base.Apply();

            Shader.UniformManager.SetColor3("fogColor", Color4.BlueViolet);

            Shader.UniformManager.SetFloat("near", Scene.Camera.depthNear);
            Shader.UniformManager.SetFloat("far", Scene.Camera.depthFar);

            Shader.UniformManager.SetFloat("fogDensity", 0.1f);
        }
    }
}
