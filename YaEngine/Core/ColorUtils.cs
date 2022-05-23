using System.Drawing;
using System.Numerics;

namespace YaEngine.Core
{
    public static class ColorUtils
    {
        private const float ToFloat01 = 1 / 255f;
        
        public static Vector3 ToVector3(this Color color)
        {
            return new(color.R * ToFloat01, color.G * ToFloat01, color.B * ToFloat01);
        }
        
        public static Vector4 ToVector4(this Color color)
        {
            return new(color.R * ToFloat01, color.G * ToFloat01, color.B * ToFloat01, color.A * ToFloat01);
        }
    }
}