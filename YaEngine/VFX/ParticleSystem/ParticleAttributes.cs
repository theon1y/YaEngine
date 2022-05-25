using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem
{
    public static class ParticleAttributes
    {
        public static readonly MeshAttribute Position = new("particlePosition", 3) { Offset = 0 };
        public static readonly MeshAttribute Rotation = new("particleRotation", 4) { Offset = 3 };
        public static readonly MeshAttribute Scale = new("particleScale", 3) { Offset = 3 + 4 };
        public static readonly MeshAttribute Color = new("particleColor", 4) { Offset = 3 + 4 + 3 };
        public static readonly MeshAttribute Uv = new("particleUv", 2) { Offset = 3 + 4 + 3 + 4 };
        
        public const int Size = 3 + 4 + 3 + 4 + 2;

        public static readonly MeshAttribute[] Attributes =
        {
            Position,
            Rotation,
            Scale,
            Color,
            Uv
        };
    }
}