using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletSharp;
using BulletSharp.Math;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Physics
{
    public class InitializeBulletPhysicsSystem : IInitializePhysicsSystem
    {
        public int Priority => InitializePriorities.First;

        private readonly CollisionConfiguration collisionConfiguration;
        private readonly BroadphaseInterface broadphase;
        private readonly ConstraintSolver? constraintSolver;
        
        public InitializeBulletPhysicsSystem(CollisionConfiguration collisionConfiguration, BroadphaseInterface broadphase,
            IEnumerable<ConstraintSolver> constraintSolverOpt)
        {
            this.collisionConfiguration = collisionConfiguration;
            this.broadphase = broadphase;
            this.constraintSolver = constraintSolverOpt.FirstOrDefault();
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            var dispatcher = new CollisionDispatcher(collisionConfiguration);
            var collisionWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, constraintSolver,
                collisionConfiguration);
            collisionWorld.Gravity = new Vector3(0, -9.81f, 0);
            world.AddSingleton<Physics>(new BulletPhysics(collisionWorld, dispatcher, broadphase,
                constraintSolver, collisionConfiguration));
            return Task.CompletedTask;
        }
    }
}