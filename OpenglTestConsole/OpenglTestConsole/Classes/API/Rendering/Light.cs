using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes.api.rendering
{
    public class Light
    {

        public Vector3 Location = Vector3.Zero;
        public Vector4 Color = Vector4.Zero;
        public Vector4 Ambient = Vector4.Zero;

        public Light(Vector3 location, Vector4 color, Vector4 ambient)
        {
            Location = location;
            Color = color;
            Ambient = ambient;
        }
    }
}
