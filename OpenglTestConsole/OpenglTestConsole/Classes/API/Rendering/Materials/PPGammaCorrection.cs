using OpenglTestConsole.Classes.API.SceneFolder;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    internal class PPGammaCorrection : PostProcessingMaterial
    {
        public PPGammaCorrection() { this.PPShader = Resources.Shaders[ResourcePaths.Shaders.PPGammaCorrection.Name]; }

        public override void Apply()
        {
            base.Apply();
            Shader.UniformManager.SetFloat("gamma", Settings.Gamma);
        }


    }
}
