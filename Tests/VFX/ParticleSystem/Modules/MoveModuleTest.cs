using System.Linq;
using System.Numerics;
using Xunit;
using YaEngine.VFX.ParticleSystem;
using YaEngine.VFX.ParticleSystem.Modules;

namespace Tests.VFX.ParticleSystem.Modules
{
    public class MoveModuleTest
    {
        [Fact]
        public void ShouldMoveParticles()
        {
            const int count = 3;
            var moves = Enumerable.Range(0, count)
                .Select(i =>
                {
                    var arr = new float[count];
                    arr[i] = 1;
                    return new Vector3(arr);
                })
                .ToArray();

            var storage = new ParticleStorage(count) { UsedParticles = count };
            for (var i = 0; i < count; ++i)
            {
                storage[i].Direction = moves[i];
                storage[i].Speed = 1;
            }

            var module = new MoveModule();
            
            module.Process(storage, 1);
            for (var i = 0; i < count; ++i)
            {
                Assert.Equal(moves[i], storage[i].Position);
            }
            
            module.Process(storage, 1);
            for (var i = 0; i < count; ++i)
            {
                Assert.Equal(2 * moves[i], storage[i].Position);
            }
        }
    }
}