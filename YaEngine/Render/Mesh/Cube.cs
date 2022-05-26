using System.Collections.Generic;

namespace YaEngine.Render
{
    public class Cube
    {
        public static readonly Mesh Mesh = new()
        {
            Vertices = new[]
            {
                0.5f, 0.5f, 0.5f,       1.0f, 1.0f,     0.0f, 0.0f, 1.0f,
                0.5f, -0.5f, 0.5f,      1.0f, 0.0f,     0.0f, 0.0f, 1.0f,
                -0.5f, 0.5f, 0.5f,      0.0f, 1.0f,     0.0f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.5f,     0.0f, 0.0f,     0.0f, 0.0f, 1.0f,
                
                0.5f, 0.5f, -0.5f,      1.0f, 1.0f,     0.0f, 0.0f, -1.0f,
                0.5f, -0.5f, -0.5f,     1.0f, 0.0f,     0.0f, 0.0f, -1.0f,
                -0.5f, 0.5f, -0.5f,     0.0f, 1.0f,     0.0f, 0.0f, -1.0f,
                -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,     0.0f, 0.0f, -1.0f,
                
                0.5f, 0.5f, 0.5f,       1.0f, 1.0f,     1.0f, 0.0f, 0.0f,
                0.5f, 0.5f, -0.5f,      1.0f, 0.0f,     1.0f, 0.0f, 0.0f,
                0.5f, -0.5f, 0.5f,      0.0f, 1.0f,     1.0f, 0.0f, 0.0f,
                0.5f, -0.5f, -0.5f,     0.0f, 0.0f,     1.0f, 0.0f, 0.0f,
                
                -0.5f, 0.5f, 0.5f,      1.0f, 1.0f,     -1.0f, 0.0f, 0.0f,
                -0.5f, 0.5f, -0.5f,     1.0f, 0.0f,     -1.0f, 0.0f, 0.0f,
                -0.5f, -0.5f, 0.5f,     0.0f, 1.0f,     -1.0f, 0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,     -1.0f, 0.0f, 0.0f,
                
                0.5f, 0.5f, 0.5f,       1.0f, 1.0f,     0.0f, 1.0f, 0.0f,
                0.5f, 0.5f, -0.5f,      1.0f, 0.0f,     0.0f, 1.0f, 0.0f,
                -0.5f, 0.5f, 0.5f,      0.0f, 1.0f,     0.0f, 1.0f, 0.0f,
                -0.5f, 0.5f, -0.5f,     0.0f, 0.0f,     0.0f, 1.0f, 0.0f,
                
                0.5f, -0.5f, 0.5f,      1.0f, 1.0f,     0.0f, -1.0f, 0.0f,
                0.5f, -0.5f, -0.5f,     1.0f, 0.0f,     0.0f, -1.0f, 0.0f,
                -0.5f, -0.5f, 0.5f,     0.0f, 1.0f,     0.0f, -1.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,     0.0f, -1.0f, 0.0f,
            },
            Indexes = new uint[]
            {
                2, 1, 0,
                2, 3, 1,
                
                4, 5, 6,
                5, 7, 6,
                
                10, 9, 8,
                10, 11, 9,
                
                12, 13, 14,
                13, 15, 14,
                
                16, 17, 18,
                17, 19, 18,
                
                22, 21, 20,
                22, 23, 21,
            },
            VertexSize = 8,
            Attributes = new List<MeshAttribute>
            {
                MeshAttributes.Position, MeshAttributes.Uv0.WithOffset(3), MeshAttributes.Normal.WithOffset(5)
            }
        };
    }
}