using BulletSharp;
using Microsoft.Extensions.DependencyInjection;
using YaEngine.Physics;

namespace YaEngine.Bootstrap
{
    public static class BulletPhysicsConfigurationExtensions
    {
        public static IServiceCollection AddBulletPhysics(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializePhysicsSystem, InitializeBulletPhysicsSystem>()
                .AddScoped<IPhysicsSystem, CreateBulletShapesSystem>()
                .AddScoped<IPhysicsSystem, ApplyBulletRigidBodySystem>()
                .AddScoped<CollisionConfiguration>(_ => new DefaultCollisionConfiguration())
                .AddScoped<BroadphaseInterface, DbvtBroadphase>()
                .AddScoped<ConstraintSolver, SequentialImpulseConstraintSolver>()
                .AddScoped<IDisposePhysicsSystem, DisposeBulletPhysicsSystem>();
        }
    }
}