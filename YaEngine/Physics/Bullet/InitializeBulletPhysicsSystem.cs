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
        private readonly CollisionDispatcher dispatcher;
        private readonly ConstraintSolver? constraintSolver;
        private readonly DiscreteDynamicsWorld collisionWorld;
        
        public InitializeBulletPhysicsSystem(CollisionConfiguration collisionConfiguration, BroadphaseInterface broadphase,
            CollisionDispatcher dispatcher, IEnumerable<ConstraintSolver> constraintSolverOpt)
        {
            this.collisionConfiguration = collisionConfiguration;
            this.broadphase = broadphase;
            this.dispatcher = dispatcher;
            this.constraintSolver = constraintSolverOpt.FirstOrDefault();
            collisionWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, constraintSolver,
                collisionConfiguration)
            {
                Gravity = new Vector3(0, -9.81f, 0)
            };
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton<Physics>(new BulletPhysics(collisionWorld));
            return Task.CompletedTask;
        }
    }
}