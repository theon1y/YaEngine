using BulletSharp;
using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Physics;

namespace YaEngine.Bootstrap
{
    public static class BulletPhysicsConfigurationExtensions
    {
        public static IServiceCollection AddBulletPhysics(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeBulletPhysicsSystem>()
                .AddScoped<IUpdateSystem, CreateBulletShapesSystem>()
                .AddScoped<IUpdateSystem, ApplyBulletRigidBodySystem>()
                .AddSingleton<CollisionConfiguration>(_ => new DefaultCollisionConfiguration())
                .AddSingleton<CollisionDispatcher>()
                .AddSingleton<BroadphaseInterface, DbvtBroadphase>()
                .AddSingleton<ConstraintSolver, SequentialImpulseConstraintSolver>()
                .AddScoped<IDisposeSystem, DisposeBulletPhysicsSystem>();
        }
    }
}