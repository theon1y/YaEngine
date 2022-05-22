using System.Numerics;
using Silk.NET.Assimp;
using YaEngine.Animation;

namespace YaEngine.Import
{
    public class AnimationImporterParts
    {
        public static readonly IAnimationImporterPart[] ImporterParts =
        {
            new AnimationPositionsImporter(),
            new AnimationRotationsImporter(),
            new AnimationScalesImporter()
        };
        
        public class AnimationPositionsImporter : AnimationImporterPart<Vector3>
        {
            protected override unsafe uint GetKeyCount(NodeAnim* channel) => channel->MNumPositionKeys;
            protected override unsafe Vector3 GetValue(NodeAnim* channel, int i) => channel->MPositionKeys[i].MValue;
            protected override unsafe double GetTime(NodeAnim* channel, int i) => channel->MPositionKeys[i].MTime;
            protected override void SetResult(BoneAnimation target, Key<Vector3>[] result) => target.Positions = result;
        }
        
        public class AnimationRotationsImporter : AnimationImporterPart<Quaternion>
        {
            protected override unsafe uint GetKeyCount(NodeAnim* channel) => channel->MNumRotationKeys;
            protected override unsafe Quaternion GetValue(NodeAnim* channel, int i) => channel->MRotationKeys[i].MValue;
            protected override unsafe double GetTime(NodeAnim* channel, int i) => channel->MRotationKeys[i].MTime;
            protected override void SetResult(BoneAnimation target, Key<Quaternion>[] result) => target.Rotations = result;
        }
        
        public class AnimationScalesImporter : AnimationImporterPart<Vector3>
        {
            protected override unsafe uint GetKeyCount(NodeAnim* channel) => channel->MNumScalingKeys;
            protected override unsafe Vector3 GetValue(NodeAnim* channel, int i) => channel->MScalingKeys[i].MValue;
            protected override unsafe double GetTime(NodeAnim* channel, int i) => channel->MScalingKeys[i].MTime;
            protected override void SetResult(BoneAnimation target, Key<Vector3>[] result) => target.Scales = result;
        }
    }
}