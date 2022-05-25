namespace YaEngine.VFX.ParticleSystem
{
    public interface IModule
    {
        void Process(ParticleStorage particles, float deltaTime);
    }
}