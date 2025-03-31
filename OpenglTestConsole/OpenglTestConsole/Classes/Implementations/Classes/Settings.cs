using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Classes
{
    public class Settings
    {
        public static float MouseSensitivity { get; set; } = 0.2f;
        // The field of view (FOV) is the vertical angle of the camera view.
        public static float FOVRadian = MathHelper.PiOver2;
        public static float Fov
        {
            get => MathHelper.RadiansToDegrees(FOVRadian);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                FOVRadian = MathHelper.DegreesToRadians(angle);
            }
        }


    }
}
