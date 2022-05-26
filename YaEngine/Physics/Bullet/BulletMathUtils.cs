using System.Numerics;
using BulletSharp.Math;
using Vector3 = BulletSharp.Math.Vector3;

namespace YaEngine.Physics
{
    public static class BulletMathUtils
    {
        public static Vector3 ToBullet(this System.Numerics.Vector3 v)
        {
            return new(v.X, v.Y, v.Z);
        }

        public static System.Numerics.Vector3 ToNative(this Vector3 v)
        {
            return new((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Matrix ToBullet(this Matrix4x4 m)
        {
            return new(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44);
        }

        public static Matrix4x4 ToNative(this Matrix m)
        {
            return new(
                (float)m.M11, (float)m.M12, (float)m.M13, (float)m.M14,
                (float)m.M21, (float)m.M22, (float)m.M23, (float)m.M24,
                (float)m.M31, (float)m.M32, (float)m.M33, (float)m.M34,
                (float)m.M41, (float)m.M42, (float)m.M43, (float)m.M44);
        }
    }
}