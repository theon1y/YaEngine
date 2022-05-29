using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Bootstrap;
using YaEngine.Core;
using YaEngine.Model;

namespace YaEngine.Animation
{
    public class AnimatorUpdateSystem : IModelSystem
    {
        public UpdateStep UpdateStep => ModelSteps.EarlyUpdate;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out Time time)) return;
            
            world.ForEach((Entity _, Animator animator) =>
            {
                animator.Update(time.DeltaTime);
            });
        }
    }
}