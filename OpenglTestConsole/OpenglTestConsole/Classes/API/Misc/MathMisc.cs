namespace OpenglTestConsole.Classes.API.Misc
{
    public class MathMisc
    {
        public static float PI = (float)Math.PI;

        public static float Cosf(double degrees) => (float)Math.Cos(degrees * Math.PI / 180f);

        public static float Sinf(double degrees) => (float)Math.Sin(degrees * Math.PI / 180f);

        public static float Atan2f(double x, double y) =>
            (float)Math.Atan2(x * Math.PI / 180f, y * Math.PI / 180f);

        public static float Asinf(double degrees) => (float)Math.Asin(degrees * Math.PI / 180f);

        public static float Atan2fRad(double x, double y) => (float)Math.Atan2(x, y);

        public static float AsinfRad(double x) => (float)Math.Asin(x);

        public static float CosfRad(double x) => (float)Math.Cos(x);

        public static float SinfRad(double x) => (float)Math.Sin(x);
    }
}
