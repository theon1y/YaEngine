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
        
        private readonly Matrix4x4[] worldTransforms;
        private readonly Matrix4x4 globalInverseTransform;
        private readonly Avatar avatar;
        
        public Animator(IEnumerable<Animation> animations, Avatar avatar)
        {
            this.avatar = avatar;
            Animations = animations.ToDictionary(x => x.Name);
            BoneMatrices = Enumerable.Repeat(Matrix4x4.Identity, MaxBones).ToArray();
            worldTransforms = new Matrix4x4[avatar.Hierarchy.Length];
            if (avatar.Hierarchy.Length > 0)
            {
                Matrix4x4.Invert(avatar.Hierarchy[0].LocalTransform, out globalInverseTransform);
            }
            else
            {
                globalInverseTransform = Matrix4x4.Identity;
            }
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
            RecalculateBoneTransforms(avatar, Animation, Time, avatar.Bones, BoneMatrices, worldTransforms,
                globalInverseTransform);
        }
        
        private static void RecalculateBoneTransforms(Avatar avatar, Animation animation, float time,
            Dictionary<string, Bone>? meshBones, Matrix4x4[] boneMatrices, Matrix4x4[] worldTransforms,
            Matrix4x4 globalInverseTransform)
        {
            for (var i = 0; i < avatar.Hierarchy.Length; ++i)
            {
                var node = avatar.Hierarchy[i];
                var parentTransform = node.ParentIndex == -1 ? globalInverseTransform : worldTransforms[node.ParentIndex];
                Matrix4x4 localTransform;
                if (animation.BoneAnimations.TryGetValue(node.Name, out var boneAnimation))
                {
                    localTransform = boneAnimation.GetLocalTransformAtTime(time);
                }
                else
                {
                    localTransform = node.LocalTransform;
                }
                var worldTransform = localTransform * parentTransform;
                worldTransforms[i] = worldTransform;
                
                if (meshBones == null || !meshBones.TryGetValue(node.Name, out var bone)) continue;

                boneMatrices[bone.Id] = bone.BoneOffset * worldTransform;
            }
        }
    }
}