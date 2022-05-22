using System;
using System.Collections.Generic;
using System.Numerics;

namespace YaEngine.Animation
{
    public class BoneAnimation
    {
        public Key<Vector3>[] Positions;
        public Key<Quaternion>[] Rotations;
        public Key<Vector3>[] Scales;
        public string Name;

        public Matrix4x4 GetLocalTransformAtTime(float animationTime)
        {
            var translation = GetTranslationAtTime(animationTime);
            var rotation = GetRotationAtTime(animationTime);
            var scale = GetScaleAtTime(animationTime);
            return rotation * scale * translation;
        }

        private Matrix4x4 GetTranslationAtTime(float animationTime)
        {
            var position = Interpolate(Positions, animationTime, Vector3.Lerp);
            return Matrix4x4.CreateTranslation(position);
        }

        private Matrix4x4 GetRotationAtTime(float animationTime)
        {
            var rotation = Interpolate(Rotations, animationTime, Quaternion.Slerp);
            return Matrix4x4.CreateFromQuaternion(rotation);
        }

        private Matrix4x4 GetScaleAtTime(float animationTime)
        {
            var scale = Interpolate(Scales, animationTime, Vector3.Lerp);
            return Matrix4x4.CreateScale(scale);
        }

        private T Interpolate<T>(Key<T>[] keys, float animationTime, Func<T, T, float, T> interpolate)
        {
            var key1Index = GetKeyFrameIndex(keys, animationTime);
            var key2Index = key1Index + 1;
            var (value1, time1) = keys[key1Index];
            var (value2, time2) = keys[key2Index];
            
            var animationDuration = time2 - time1;
            var animatedTime = animationTime - time1;
            var t = animatedTime / animationDuration;
            
            return interpolate(value1, value2, t);
        }

        private int GetKeyFrameIndex<T>(Key<T>[] keys, float animationTime)
        {
            for (var i = 0; i < keys.Length - 1; ++i)
            {
                if (animationTime < keys[i].Time) return i;
            }

            return keys.Length - 2;
            throw new ArgumentException($"Could not find keyframe for {animationTime} time in {Name}");
        }
    }
}