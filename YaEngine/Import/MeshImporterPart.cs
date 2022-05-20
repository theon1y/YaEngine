using YaEngine.Render;
using ImportMesh = Silk.NET.Assimp.Mesh;

namespace YaEngine.Import
{
    public abstract class MeshImporterPart<T> : IMeshImporterPart where T : unmanaged
    {
        public abstract int Size { get; }
        
        public unsafe int TryAppendSize(ImportMesh* pMesh, Mesh mesh, int offset)
        {
            if (GetData(pMesh) == null) return offset;

            WriteOffset(mesh, offset);
            return offset + Size;
        }

        public unsafe int TryCopyTo(ImportMesh* pMesh, int i, float[] vertexData, int offset)
        {
            if (GetData(pMesh) == null) return offset;

            var position = GetData(pMesh)[i];
            CopyTo(position, vertexData, offset);
            return offset + Size;
        }

        protected abstract unsafe T* GetData(ImportMesh* pMesh);
        protected abstract void WriteOffset(Mesh mesh, int offset);
        protected abstract void CopyTo(T value, float[] vertexData, int offset);
    }

    public interface IMeshImporterPart
    {
        unsafe int TryAppendSize(ImportMesh* pMesh, Mesh mesh, int offset);
        unsafe int TryCopyTo(ImportMesh* pMesh, int i, float[] vertexData, int offset);
    }
}