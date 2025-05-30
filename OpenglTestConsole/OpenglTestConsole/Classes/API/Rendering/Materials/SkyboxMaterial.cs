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
            this.Shader = Resources.Shaders[ResourcePaths.Materials.Skybox.Name];
            this.cubemap = cubemap;
        }
        public override void Apply()
        { // set shader values here
            Shader.UniformManager.SetCubemap("skybox", cubemap, TextureUnit.Texture0);
        }
    }
}
