using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Bootstrap;

namespace YaEngine.Animation
{
    public class AnimatorUpdateSystem : IUpdateSystem
    {
        public UpdateStep UpdateStep => UpdateSteps.EarlyUpdate;
        
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