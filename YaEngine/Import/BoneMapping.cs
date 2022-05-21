using System.Collections.Generic;
using YaEngine.Animation;

namespace YaEngine.Import
{
    public record BoneMapping(Dictionary<string, Bone> Bones, Dictionary<uint, List<BoneWeight>> Weights);

    public record BoneWeight(int BoneId, float Weight);
}