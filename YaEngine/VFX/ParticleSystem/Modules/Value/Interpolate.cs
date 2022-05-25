using System.Drawing;
using System.Numerics;
using YaEngine.Core;

namespace YaEngine.VFX.ParticleSystem.Modules.Value
{
    public class InterpolateFloat : IValueProvider<float>
    {
        private readonly float value1;
        private readonly float value2;
        
        public InterpolateFloat(float value1, float value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public float Get(float t)
        {
            return value1 * t + value2 * (1 - t);
        }
    }
    
    public class InterpolateVector2 : IValueProvider<Vector2>
    {
        private readonly Vector2 value1;
        private readonly Vector2 value2;
        
        public InterpolateVector2(Vector2 value1, Vector2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public Vector2 Get(float t)
        {
            return Vector2.Lerp(value1, value2, t);
        }
    }
    
    public class InterpolateVector3 : IValueProvider<Vector3>
    {
        private readonly Vector3 value1;
        private readonly Vector3 value2;
        
        public InterpolateVector3(Vector3 value1, Vector3 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public Vector3 Get(float t)
        {
            return Vector3.Lerp(value1, value2, t);
        }
    }
    
    public class InterpolateVector4 : IValueProvider<Vector4>
    {
        private readonly Vector4 value1;
        private readonly Vector4 value2;
        
        public InterpolateVector4(Vector4 value1, Vector4 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public Vector4 Get(float t)
        {
            return Vector4.Lerp(value1, value2, t);
        }
    }
    
    public class InterpolateQuaternion : IValueProvider<Quaternion>
    {
        private readonly Quaternion value1;
        private readonly Quaternion value2;
        
        public InterpolateQuaternion(Quaternion value1, Quaternion value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public Quaternion Get(float t)
        {
            return Quaternion.Slerp(value1, value2, t);
        }
    }
}