using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Model;

namespace YaEngine.Render
{
    public class RegisterCameraSystem : IModelSystem
    {
        public UpdateStep UpdateStep => ModelSteps.EarlyUpdate;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out CameraRegistry registry)) return;

            registry.Cameras.Clear();
            world.ForEach((Entity entity, Camera _) =>
            {
                registry.Cameras.Add(entity);
            });
        }
    }
}