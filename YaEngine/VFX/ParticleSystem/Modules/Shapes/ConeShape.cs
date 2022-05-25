using System;
using System.Numerics;
using YaEngine.Core;

namespace YaEngine.VFX.ParticleSystem.Modules.Shapes
{
    public class ConeShape : IShape
    {
        private readonly float angle;
        private readonly float height;
        private readonly float radius;

        public ConeShape(float angleDegrees, float height = 1, float radius = 1)
        {
            this.angle = angleDegrees.ToRadians();
            this.height = height;
            this.radius = radius;
        }
        
        public Vector3 GetDirection(float t)
        {
            var x = (1 - 2 * t) * radius * MathF.Cos(angle);
            var y = height;
            var z = (1 - 2 * t) * radius * MathF.Sin(angle);
            return new Vector3(x, y, z);
        }
    }
}