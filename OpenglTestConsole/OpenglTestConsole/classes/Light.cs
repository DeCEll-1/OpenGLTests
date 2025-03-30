using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Light
    {

        public Vector3 Location = Vector3.Zero;
        public Vector3 Color = Vector3.Zero;

        public Light(Vector3 location, Vector3 color)
        {
            Location = location;
            Color = color;
        }
    }
}
