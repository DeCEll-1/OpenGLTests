﻿using System.Globalization;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Extensions
{
    public static class VectorExtensions
    {
        #region m*th
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

        /// <summary>
        /// top left , top right, bottom left, bottom right
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3[] CreateSquare(this Vector3 vector)
        {
            return
            [
                new(-vector.X / 2f, vector.Y / 2f, 0f),
                new(vector.X / 2f, vector.Y / 2f, 0f),
                new(-vector.X / 2f, -vector.Y / 2f, 0f),
                new(vector.X / 2f, -vector.Y / 2f, 0f),
            ];
        }

        /// <summary>
        /// direction vector to eulers radians
        /// </summary>
        /// <param name="directionVector"></param>
        /// <returns></returns>
        public static Vector3 DirectionToEulerRadians(this Vector3 directionVector)
        {
            directionVector = Vector3.Normalize(directionVector);
            float yaw = MathF.Atan2(directionVector.X, directionVector.Z);
            float pitch = MathF.Asin(-directionVector.Y);
            float roll = 0f;
            return new Vector3(roll, pitch, yaw);
        }

        /// <summary>
        /// takes radians and outputs direction vector
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static Vector3 TurnToDirectionVector(this Vector3 radians)
        {
            Vector3 outVector = new Vector3();
            outVector.X = MathF.Cos(radians.Z) * MathF.Cos(radians.Y);
            outVector.Y = MathF.Sin(radians.Z) * MathF.Cos(radians.Y);
            outVector.Z = MathF.Sin(radians.Y);
            return outVector;
        }
        #endregion

        #region colors
        public static Vector4 ToVector4(this Color4 color) =>
            new(color.R, color.G, color.B, color.A);

        public static Color4 ToColor4(this Vector4 color) =>
            new(color.X, color.Y, color.Z, color.W);

        public static string ToHex(this Vector4 color) =>
            $"{(int)(color.X * 255):X2}{(int)(color.Y * 255):X2}{(int)(color.Z * 255):X2}{(int)(color.W * 255):X2}";

        public static Vector4 FromHex(string hex) =>
            new Vector4(
                byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) / 255f,
                byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber) / 255f,
                byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber) / 255f,
                byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) / 255f
            );
        #endregion
    }
}
