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
        public MonoColorMaterial(Vector4 col)
        {
            this.color = col;
            this.Shader = Resources.Shaders[ResourcePaths.Materials.MonoColor.Name];
        }
        public override void Apply()
        {
            Shader.UniformManager.SetVector4("color", color);
        }
    }
}
