using System.Collections.Generic;

namespace YaEngine.Animation
{
    public class Animation
    {
        public string Name;
        public float Duration;
        public int FramesPerSecond;
        public Dictionary<string, BoneAnimation> BoneAnimations;
    }
}