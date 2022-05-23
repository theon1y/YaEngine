using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using YaEcs;

namespace YaEngine.Animation
{
    public class Animator : IComponent
    {
        public const int MaxBones = 64;
        
        public float Time;
        public Animation? Animation;
        public readonly Dictionary<string, Animation> Animations;
        public readonly Matrix4x4[] BoneMatrices;
        
        private readonly Matrix4x4[] worldMatrices;
        private readonly Avatar avatar;
        
        public Animator(IEnumerable<Animation> animations, Avatar avatar)
        {
            this.avatar = avatar;
            Animations = animations.ToDictionary(x => x.Name);
            BoneMatrices = new Matrix4x4[MaxBones];
            worldMatrices = new Matrix4x4[avatar.Hierarchy.Length];
        }

        public void Play(string animationName)
        {
            Time = 0;
            Animation = Animations.GetValueOrDefault(animationName);
        }

        public void Update(float deltaTime)
        {
            if (Animation == null) return;

            Time = (Time + Animation.FramesPerSecond * deltaTime) % Animation.Duration;
            RecalculateBoneTransforms(avatar, Animation, Time, avatar.Bones, BoneMatrices, worldMatrices);
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