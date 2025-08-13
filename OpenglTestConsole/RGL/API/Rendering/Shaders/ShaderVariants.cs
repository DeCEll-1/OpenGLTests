using RGL.Classes.API.Rendering.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.Rendering.Shaders
{
    public class ShaderVariants
    {
        public Shader Opaque { get; }
        public Shader Transparent { get; }

        public ShaderVariants(Shader opaque, Shader transparent)
        {
            Opaque = opaque;
            Transparent = transparent;
        }
    }
}
