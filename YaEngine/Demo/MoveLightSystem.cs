using System;
using System.Numerics;
using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Bootstrap;
using YaEngine.Core;

namespace YaEngine
{
    public class MoveLightSystem : IUpdateSystem
    {
        public UpdateStep UpdateStep => UpdateSteps.Update;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out Time time)) return;

            const float speed = 2;
            const float size = 10;
            var sin = (float) Math.Sin(time.TimeSinceStartup * speed);
            var sin2 = (float) Math.Sin(time.TimeSinceStartup * speed * 2);
            var cos = (float) Math.Cos(time.TimeSinceStartup * speed);
            var cos2 = (float) Math.Cos(time.TimeSinceStartup * speed * 2);
            world.ForEach((Entity _, SpotLight _, Transform transform) =>
            {
                // transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float)time.TimeSinceStartup);
                // return;
                var position = transform.Position;
                position.X = sin2 * size;
                position.Z = -cos2 * size;
                position.Y = sin * size;
                transform.Position = position;
            });
        }
    }
}