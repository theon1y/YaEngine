using System.Collections.Generic;
using System.Numerics;
using YaEcs;

namespace YaEngine.Core
{
    public class Transform : IComponent
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation = Quaternion.Identity;
        public Transform? Parent;
        public List<Transform> Children = new();

        public Matrix4x4 ModelMatrix
        {
            get
            {
                var parentModel = Parent?.ModelMatrix ?? Matrix4x4.Identity;
                var rotation = Matrix4x4.CreateFromQuaternion(Rotation);
                var scale = Matrix4x4.CreateScale(Scale);
                var translation = Matrix4x4.CreateTranslation(Position);
                return rotation * scale * translation * parentModel;
            }
        }
    }
}