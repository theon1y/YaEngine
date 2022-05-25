namespace YaEngine.VFX.ParticleSystem.Modules.Value
{
    public interface IValueProvider<T>
    {
        T Get(float t);
    }
}