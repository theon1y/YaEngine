namespace YaEngine.VFX.ParticleSystem.Modules.Value
{
    public class Constant<T> : IValueProvider<T>
    {
        private readonly T value;

        public Constant(T value)
        {
            this.value = value;
        }
        
        public T Get(float t)
        {
            return value;
        }
    }
}