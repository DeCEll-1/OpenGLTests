using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Misc
{
    public class MathMisc
    {
        public static float PI = (float)Math.PI;
        public static Func<double, float> cosf = delegate (double x) { return (float)Math.Cos(x * PI / 180f); };
        public static Func<double, float> sinf = delegate (double x) { return (float)Math.Sin(x * PI / 180f); };
        public static Func<double, float> cosfRad = delegate (double x) { return (float)Math.Cos(x); };
        public static Func<double, float> sinfRad = delegate (double x) { return (float)Math.Sin(x); };
    }
}
