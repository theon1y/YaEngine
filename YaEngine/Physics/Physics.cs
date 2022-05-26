using System.Numerics;
using YaEcs;

namespace YaEngine.Physics
{
    public abstract class Physics : IComponent
    {
        public abstract void Update(float deltaTime);
        public abstract Raycast Raycast(Vector3 from, Vector3 to);
    }
}