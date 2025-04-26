using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class Light
    {

        public Vector3 Position = Vector3.Zero;
        public Vector3 Color = Vector3.Zero;

        public Light(Vector3 location, Vector3 color)
        {
            Position = location;
            Color = color;
        }
    }
}
