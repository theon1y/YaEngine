using YaEngine.Render;
using ImportMesh = Silk.NET.Assimp.Mesh;

namespace YaEngine.Import
{
    public abstract class MeshImporterPart<T> : IMeshImporterPart where T : unmanaged
    {
        protected abstract MeshAttribute Attribute { get; }

        public int Size => Attribute.Size;

        public unsafe bool HasAttribute(ImportMesh* pMesh)
        {
            var data = GetData(pMesh);
            return data != null;
        }
        
        public void AddAttribute(Mesh mesh, int offset)
        {
            mesh.Attributes.Add(Attribute.WithOffset(offset));
        }

        public unsafe void CopyTo(ImportMesh* pMesh, int i, float[] vertexData, int offset)
        {
            var position = GetData(pMesh)[i];
            CopyTo(position, vertexData, offset);
        }

        protected abstract unsafe T* GetData(ImportMesh* pMesh);
        protected abstract void CopyTo(T value, float[] vertexData, int offset);
    }

    public interface IMeshImporterPart
    {
        int Size { get; }
        unsafe bool HasAttribute(ImportMesh* pMesh);
        void AddAttribute(Mesh mesh, int offset);
        unsafe void CopyTo(ImportMesh* pMesh, int i, float[] vertexData, int offset);
    }
}