using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    internal class SkyboxMaterial : Material
    {
        private Cubemap cubemap;
        public SkyboxMaterial(Cubemap cubemap)
        {
            this.cubemap = cubemap;
        }

        public override Shader Shader => Resources.Shaders[ResourcePaths.Shaders.Skybox.Name];

        public override void Apply()
        { // set shader values here
            Shader.UniformManager.SetCubemap("skybox", cubemap, TextureUnit.Texture0);
        }
    }
}
