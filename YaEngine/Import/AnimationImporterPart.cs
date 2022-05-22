using Silk.NET.Assimp;
using YaEngine.Animation;

namespace YaEngine.Import
{
    public abstract class AnimationImporterPart<T> : IAnimationImporterPart where T : unmanaged
    {
        public unsafe void Import(BoneAnimation target, NodeAnim* channel)
        {
            var n = GetKeyCount(channel);
            var result = new Key<T>[n];
            
            for (var i = 0; i < n; ++i)
            {
                result[i] = new Key<T>(GetValue(channel, i), (float)GetTime(channel, i));
            }

            SetResult(target, result);
        }

        protected abstract unsafe uint GetKeyCount(NodeAnim* channel);
        protected abstract unsafe T GetValue(NodeAnim* channel, int i);
        protected abstract unsafe double GetTime(NodeAnim* channel, int i);
        protected abstract void SetResult(BoneAnimation target, Key<T>[] result);
    }

    public interface IAnimationImporterPart
    {
        unsafe void Import(BoneAnimation target, NodeAnim* channel);
    }
}