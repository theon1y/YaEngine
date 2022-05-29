using System.Linq;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.SceneManagement
{
    public class DisposeEntitiesSystem : IDisposeModelSystem
    {
        public int Priority => DisposePriorities.Fifth;
        
        public Task ExecuteAsync(IWorld world)
        {
            var entities = world.Entities.ToList();
            foreach (var entity in entities)
            {
                world.DeleteEntity(entity);
            }
            entities.Clear();
            
            return Task.CompletedTask;
        }
    }
}