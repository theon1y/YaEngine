using System;

namespace YaEngine.VFX.ParticleSystem.Modules.Value
{
    public class Random<T> : IValueProvider<T>
    {
        private readonly Random random;
        private readonly T value1;
        private readonly T value2;

        public Random(Random random, T value1, T value2)
        {
            this.random = random;
            this.value1 = value1;
            this.value2 = value2;
        }

        public T Get(float t)
        {
            var proc = random.NextSingle();
            if (proc >= 0.5f) return value2;

            return value1;
        }
    }
}