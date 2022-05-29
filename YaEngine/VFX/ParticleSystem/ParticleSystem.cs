using System.Numerics;
using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Core;
using YaEngine.Model;
using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem
{
    public class ParticleSystem : IModelSystem
    {
        public UpdateStep UpdateStep => ModelSteps.Update;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out Time time)) return;

            world.ForEach((Entity _, ParticleEffect effect, ParticleRenderer particleRenderer, Renderer renderer) =>
            {
                if (renderer.InstanceData == null) return;
                
                if (!particleRenderer.Particles.IsPlaying)
                {
                    renderer.IsEnabled = false;
                    return;
                }

                renderer.IsEnabled = true;
                foreach (var module in effect.Modules)
                {
                    module.Process(particleRenderer.Particles, time.DeltaTime);
                }
                
                Rebind(particleRenderer.Particles, particleRenderer.Mesh.Vertices);
                renderer.InstanceCount = (uint) particleRenderer.Particles.UsedParticles;
            });
        }

        private static void Rebind(ParticleStorage particles, float[] buffer)
        {
            for (var i = 0; i < particles.UsedParticles; ++i)
            {
                ref var particle = ref particles.Particles[i];
                var attributeOffset = i * ParticleAttributes.Size;
                
                particle.Position.CopyTo(buffer, attributeOffset + ParticleAttributes.Position.Offset);
                CopyQuaternion(particle.Rotation, buffer, attributeOffset + ParticleAttributes.Rotation.Offset);
                particle.Scale.CopyTo(buffer, attributeOffset + ParticleAttributes.Scale.Offset);
                particle.Color.CopyTo(buffer, attributeOffset + ParticleAttributes.Color.Offset);
                particle.Uv.CopyTo(buffer, attributeOffset + ParticleAttributes.Uv.Offset);
            }
        }

        private static void CopyQuaternion(Quaternion q, float[] buffer, int offset)
        {
            buffer[offset] = q.X;
            buffer[offset + 1] = q.Y;
            buffer[offset + 2] = q.Z;
            buffer[offset + 3] = q.W;
        }
    }
}