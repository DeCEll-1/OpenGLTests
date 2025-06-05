using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.Implementations.Classes
{
    public class Settings
    {
        public static float MouseSensitivity { get; set; } = 0.1f;

        // The field of view (FOV) is the vertical angle of the camera view.
        public static float FOVRadian = MathHelper.PiOver2;
        public static float Fov
        {
            get => MathHelper.RadiansToDegrees(FOVRadian);
            set { FOVRadian = MathHelper.DegreesToRadians(value); }
        }
        public static float Gamma = 2.2f;
        public static Vector2i Resolution { get; set; } = new(800, 800);
    }
}
