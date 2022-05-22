using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using YaEcs;

namespace YaEngine.Animation
{
    public class Animator : IComponent
    {
        public const int MaxBones = 64;
        
        private readonly Dictionary<string, Animation> animations;
        private readonly Matrix4x4[] worldMatrices;
        private readonly Avatar avatar;
        private float time;
        private Animation? animation;
        
        public Animator(IEnumerable<Animation> animations, Avatar avatar)
        {
            this.animations = animations.ToDictionary(x => x.Name);
            this.avatar = avatar;
            BoneMatrices = new Matrix4x4[MaxBones];
            worldMatrices = new Matrix4x4[avatar.Hierarchy.Length];
        }
        
        public Matrix4x4[] BoneMatrices { get; }

        public void Play(string animationName)
        {
            time = 0;
            animation = animations.GetValueOrDefault(animationName);
        }

        public void Update(float deltaTime)
        {
            if (animation == null) return;

            time = (time + animation.FramesPerSecond * deltaTime) % animation.Duration;
            RecalculateBoneTransforms(avatar, animation, time, avatar.Bones, BoneMatrices, worldMatrices);
        }

        private static void RecalculateBoneTransforms(Avatar avatar, Animation animation, float time,
            Dictionary<string, Bone>? meshBones, Matrix4x4[] boneMatrices, Matrix4x4[] worldMatrices)
        {
            for (var i = 0; i < avatar.Hierarchy.Length; ++i)
            {
                var node = avatar.Hierarchy[i];

                var parentTransform = node.ParentIndex == -1 ? Matrix4x4.Identity : worldMatrices[node.ParentIndex];
                var worldTransform = parentTransform;
                if (animation.BoneAnimations.TryGetValue(node.Name, out var boneAnimation))
                {
                    var localTransform = boneAnimation.GetLocalTransformAtTime(time);
                    worldTransform = localTransform * parentTransform;
                }

                worldMatrices[i] = worldTransform;

                if (meshBones != null && meshBones.TryGetValue(node.Name, out var bone))
                {
                    boneMatrices[bone.Id] = bone.BoneOffset * worldTransform;
                }
            }
        }
    }
}