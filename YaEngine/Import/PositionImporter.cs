using System.Numerics;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public class PositionImporter : MeshImporterPart<Vector3>
    {
        public override int Size => 3;
        protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.PositionOffset = offset;
        protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MVertices;
        protected override void CopyTo(Vector3 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
    }
}