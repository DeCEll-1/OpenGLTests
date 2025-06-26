global using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using RGL.API.Attributes;

namespace RGL.API
{
    public class APISettings : ReflectiveSettings
    {
        public static float MouseSensitivity { get; set; } = 0.1f;
        // The field of view (FOV) is the vertical angle of the camera view.
        [DoNotSave]
        public static float FOVRadian { get; set; } = MathHelper.PiOver2;
        public static float Fov
        {
            get => MathHelper.RadiansToDegrees(FOVRadian);
            set { FOVRadian = MathHelper.DegreesToRadians(value); }
        }
        public static float Gamma { get; set; } = 2.2f;
        public static Vector2i Resolution { get; set; } = new(1600, 900);
        public static float CameraDepthNear { get; set; } = 0.1f;

        public static float CameraDepthFar { get; set; } = 400f;
    }
}
