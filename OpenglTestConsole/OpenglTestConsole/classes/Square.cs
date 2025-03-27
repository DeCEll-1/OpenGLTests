using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Square
    {

        public static Vector3[] GetSquare(float width, float height)
        {
            float wDivided = width / 2f;
            float hDivided = height / 2f;

            return new Vector3[]
                    {
                        new Vector3(-wDivided, hDivided, 0f),
                        new Vector3(wDivided, hDivided, 0f),
                        new Vector3(-wDivided, -hDivided, 0f),
                        new Vector3(wDivided, -hDivided, 0f)
                    };
        }

    }
}
