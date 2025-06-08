using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Generated.Paths;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    internal class MonoColorMaterial : Material
    {
        public Vector4 color = new(1);
        public override Shader Shader => Resources.Shaders[ResourcePaths.Shaders.MonoColor.Name];
        public MonoColorMaterial(Vector4 col)
        {
            this.color = col;
        }


        public override void Apply()
        {
            Shader.UniformManager.SetVector4("color", color);
        }
    }
}
