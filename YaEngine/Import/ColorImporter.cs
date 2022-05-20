using System.Numerics;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public class ColorImporter : MeshImporterPart<Vector4>
    {
        public override int Size => 4;
        protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.ColorOffset = offset;
        protected override unsafe Vector4* GetData(Mesh* pMesh) => pMesh->MColors.Element0;
        protected override void CopyTo(Vector4 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
    }
}