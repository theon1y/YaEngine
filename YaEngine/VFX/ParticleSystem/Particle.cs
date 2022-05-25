using System.Numerics;

namespace YaEngine.VFX.ParticleSystem
{
    public struct Particle
    {
        public Vector3 Direction;
        public float Speed;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Vector2 Uv;
        public Vector4 Color;
        public float Time;
        public float NormalizedTime;
        public float Duration;
        public bool IsAlive;
        
        public void Reset()
        {
            Direction = default;
            Speed = default;
            Position = default;
            Rotation = default;
            Scale = default;
            Uv = default;
            Color = default;
            Time = default;
            NormalizedTime = default;
            Duration = default;
            IsAlive = default;
        }
    }
}