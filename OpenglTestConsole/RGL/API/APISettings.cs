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

        public static int ForceGCIntervalMS { get; set; } = 30_000;
        public static bool LogForceGC { get; set; } = true;
        public static long MinRamBytesForForcedGC { get; set; } = 50 * 1_048_576; // 50 mbs
        public static bool DisplayNonPublicVariablesForSceneDebug { get; set; } = true;

        public static bool DisplayLogsInNotifications { get; set; } = true;
        public static double LogNotificationDurationMS { get; set; } = TimeSpan.FromSeconds(15f).TotalMilliseconds;

    }
}
