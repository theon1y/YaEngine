using System.Collections.Generic;

namespace YaEngine.Render
{
    public class Quad
    {
        public static readonly Mesh Mesh = new()
        {
            Vertices = new[]
            {
                0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
            },
            Indexes = new uint[]
            {
                0, 1, 2,
                1, 3, 2
            },
            VertexSize = 5,
            Attributes = new List<MeshAttribute> { MeshAttributes.Position, MeshAttributes.Uv0.WithOffset(3) }
        };
    }
}