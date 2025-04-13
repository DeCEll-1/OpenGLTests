using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 ToRadians(this Vector3 angles)
        {
            return new Vector3(
                MathHelper.DegreesToRadians(angles.X),
                MathHelper.DegreesToRadians(angles.Y),
                MathHelper.DegreesToRadians(angles.Z)
            );
        }

        public static Vector3 ToDegrees(this Vector3 radians)
        {
            return new Vector3(
                MathHelper.RadiansToDegrees(radians.X),
                MathHelper.RadiansToDegrees(radians.Y),
                MathHelper.RadiansToDegrees(radians.Z)
            );
        }

    }
}
