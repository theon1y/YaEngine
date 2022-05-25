using System.Numerics;
using YaEngine.Render;
using Mesh = Silk.NET.Assimp.Mesh;

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
            protected override MeshAttribute Attribute => MeshAttributes.Position;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MVertices;
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private class NormalImporter : MeshImporterPart<Vector3>
        {
            protected override MeshAttribute Attribute => MeshAttributes.Normal;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MNormals;
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private class ColorImporter : MeshImporterPart<Vector4>
        {
            protected override MeshAttribute Attribute => MeshAttributes.Color;
            protected override unsafe Vector4* GetData(Mesh* pMesh) => pMesh->MColors.Element0;
            protected override void CopyTo(Vector4 value, float[] vertexData, int offset) => value.CopyTo(vertexData, offset);
        }

        private abstract class UvImporter : MeshImporterPart<Vector3>
        {
            protected override void CopyTo(Vector3 value, float[] vertexData, int offset)
            {
                vertexData[offset] = value.X;
                vertexData[offset + 1] = value.Y;
            }
        }

        private class Uv0Importer : UvImporter
        {
            protected override MeshAttribute Attribute => MeshAttributes.Uv0;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element0;
        }

        private class Uv1Importer : UvImporter
        {
            protected override MeshAttribute Attribute => MeshAttributes.Uv1;
            protected override unsafe Vector3* GetData(Mesh* pMesh) => pMesh->MTextureCoords.Element1;
        }
    }
}