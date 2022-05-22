using System.Numerics;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public class MeshImporterParts
    {
        public static readonly IMeshImporterPart[] MeshImporters =
        {
            new PositionImporter(),
            new Uv0Importer(),
            new NormalImporter(),
            new ColorImporter(),
            new Uv1Importer()
        };

        private class PositionImporter : MeshImporterPart<Vector3>
        {
            public override int Size => 3;
            protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.PositionOffset = offset;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MVertices;
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private class NormalImporter : MeshImporterPart<Vector3>
        {
            public override int Size => 3;
            protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.NormalOffset = offset;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MNormals;
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private class ColorImporter : MeshImporterPart<Vector4>
        {
            public override int Size => 4;
            protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.ColorOffset = offset;
            protected override unsafe Vector4* GetData(Mesh* pMesh) => pMesh->MColors.Element0;
            protected override void CopyTo(Vector4 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private abstract class UvImporter : MeshImporterPart<Vector3>
        {
            public override int Size => 2;
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset)
            {
                vertexData[offset] = value.X;
                vertexData[offset + 1] = value.Y;
            }
        }

        private class Uv0Importer : UvImporter
        {
            protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.Uv0Offset = offset;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element0;
        }

        private class Uv1Importer : UvImporter
        {
            protected override void WriteOffset(Render.Mesh mesh, int offset) => mesh.Uv1Offset = offset;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element1;
        }
    }
}