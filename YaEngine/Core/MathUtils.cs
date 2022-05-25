using System;
using System.Numerics;

namespace YaEngine.Core
{
    public static class MathUtils
    {
        public const float DegreesToRadians = MathF.PI / 180f;
        public const float RadiansToDegrees = 180f / MathF.PI;
        public const float Pi2f = MathF.PI * 2f;
        public const float Epsilon = 0.0001f;
        
        public static float ToRadians(this float degrees)
        {
            return DegreesToRadians * degrees;
        }
        
        public static float ToDegrees(this float radians)
        {
            return RadiansToDegrees * radians;
        }
        
        public static Vector3 ToRadians(this Vector3 degrees)
        {
            return DegreesToRadians * degrees;
        }
        
        public static Vector3 ToDegrees(this Vector3 radians)
        {
            return RadiansToDegrees * radians;
        }

        public static (float, float, float) Deconstruct(this Vector3 vector)
        {
            return (vector.X, vector.Y, vector.Z);
        }

        // yxz
        public static Vector3 ToEuler(this Quaternion q1)
        {
            var sqw = q1.W * q1.W;
            var sqx = q1.X * q1.X;
            var sqy = q1.Y * q1.Y;
            var sqz = q1.Z * q1.Z;
            var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            var test = q1.X * q1.W - q1.Y * q1.Z;
            
            Vector3 v;
            if (test > 0.4995f * unit) // singularity at north pole
            {
                v.Y = 2f * MathF.Atan2(q1.Y, q1.X);
                v.X = MathF.PI / 2f;
                v.Z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                v.Y = -2f * MathF.Atan2(q1.Y, q1.X);
                v.X = -MathF.PI / 2;
                v.Z = 0;
            }
            else
            {
                var rot = new Quaternion(q1.W, q1.Z, q1.X, q1.Y);
                // Yaw
                v.Y = MathF.Atan2(2f * rot.X * rot.W + 2f * rot.Y * rot.Z, 1 - 2f * (rot.Z * rot.Z + rot.W * rot.W));
                // Pitch
                v.X = MathF.Asin(2f * (rot.X * rot.Z - rot.W * rot.Y));
                // Roll
                v.Z = MathF.Atan2(2f * rot.X * rot.Y + 2f * rot.Z * rot.W, 1 - 2f * (rot.Y * rot.Y + rot.Z * rot.Z));
            }
            
            NormalizeAngles(ref v);
            return v;
        }

        public static Vector3 ToEulerDegrees(this Quaternion q)
        {
            var eulerRadians = q.ToEuler();
            return eulerRadians * RadiansToDegrees;
        }

        public static Quaternion FromEulerDegrees(float x, float y, float z)
        {
            return Quaternion.CreateFromYawPitchRoll(ToRadians(y), ToRadians(x), ToRadians(z));
        }
        
        private static void NormalizeAngles(ref Vector3 angles)
        {
            angles.X = NormalizeAngle(angles.X);
            angles.Y = NormalizeAngle(angles.Y);
            angles.Z = NormalizeAngle(angles.Z);
        }
        
        public static Vector3 NormalizeAngles(this Vector3 angles)
        {
            angles.X = NormalizeAngle(angles.X);
            angles.Y = NormalizeAngle(angles.Y);
            angles.Z = NormalizeAngle(angles.Z);
            return angles;
        }
 
        public static float NormalizeAngle(float angle)
        {
            while (angle > Pi2f) angle -= Pi2f;
            while (angle < 0) angle += Pi2f;
            return angle;
        }
 
        public static float NormalizeAngleDegrees(float angle)
        {
            while (angle > 360) angle -= 360;
            while (angle < 0) angle += 360;
            return angle;
        }

        public static float Interpolate(float a, float b, float t)
        {
            return a * t + b * (1 - t);
        }
    }
}