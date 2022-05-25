using YaEngine.Animation;

namespace YaEngine.Render
{
    public static class MeshAttributes
    {
        public static readonly MeshAttribute Position = new("vPos", 3);
        public static readonly MeshAttribute Uv0 = new("vUv", 2);
        public static readonly MeshAttribute Normal = new("vNormal", 3);
        public static readonly MeshAttribute Color = new("vColor", 4);
        public static readonly MeshAttribute BoneWeights = new("vBoneWeights", Bone.MaxNesting);
        public static readonly MeshAttribute BoneIds = new("vBoneIds", Bone.MaxNesting);
        public static readonly MeshAttribute Uv1 = new("vUv1", 2);

        public static MeshAttribute WithOffset(this MeshAttribute attribute, int offset)
        {
            attribute.Offset = offset;
            return attribute;
        }
    }
}