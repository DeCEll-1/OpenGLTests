using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders
{
    public abstract class Material
    {
        // ok the plan should be easy
        // so i have Mesh but i want to set each mesh to a specific material
        // i can do it in the constructor, but thats meh
        // so ill just use generalised types like Mesh<T> extends material or w/e
        public Material() { }

        public Shader Shader { get; set; }

        public abstract void Apply();

    }
}
