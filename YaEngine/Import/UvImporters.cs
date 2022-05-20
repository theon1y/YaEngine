using System.Numerics;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public abstract class UvImporter : MeshImporterPart<Vector3>
    {
        public override int Size => 2;
        protected override void CopyTo(Vector3 value, float[] vertexData, int offset)
        {
            vertexData[offset] = value.X;
            vertexData[offset + 1] = value.Y;
        }
    }
    
    public class Uv0Importer : UvImporter
    {
        protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.Uv0Offset = offset;
        protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element0;
    }
    
    public class Uv1Importer : UvImporter
    {
        protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.Uv1Offset = offset;
        protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element1;
    }
}