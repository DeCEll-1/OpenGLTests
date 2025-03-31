using OpenglTestConsole.classes;
using OpenglTestConsole.classes.api.rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    public class Square : Mesh
    {
        public Vector3 LeftTop { get; set; }
        public Vector3 RightTop { get; set; }
        public Vector3 LeftBottom { get; set; }
        public Vector3 RightBottom { get; set; }
        [SetsRequiredMembers]
        public Square(Camera camera, float size) : base(camera)
        {
            float width = size / 2f;
            float height = size / 2f;

            LeftTop = new(-width, height, 0.0f);
            RightTop = new(width, height, 0.0f);
            LeftBottom = new(-width, -height, 0.0f);
            RightBottom = new(width, -height, 0.0f);
        }

    }
}
