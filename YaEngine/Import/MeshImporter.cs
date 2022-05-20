using System;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Assimp;
using ImportMesh = Silk.NET.Assimp.Mesh;
using Mesh = YaEngine.Render.Mesh;

namespace YaEngine.Import
{
    public class MeshImporter
    {
        private readonly Assimp assimp = Assimp.GetApi();
        private readonly IMeshImporterPart[] meshImporters =
        {
            new PositionImporter(),
            new Uv0Importer(),
            new NormalImporter(),
            new ColorImporter(),
            new Uv1Importer()
        };

        public unsafe Mesh[] Import(string filePath)
        {
            var filePathGcHandle = MarshalToPinnedAscii(filePath, out var filePathPointer);
            var scenePointer = assimp.ImportFile(filePathPointer, 0);
            if (scenePointer == null)
            {
                var errorPointer = assimp.GetErrorString();
                var error = Marshal.PtrToStringUTF8(new IntPtr(errorPointer));
                throw new Exception($"Could not import {filePath}:\n{error}");
            }

            var result = ParseScene(scenePointer);
            
            assimp.ReleaseImport(scenePointer);
            filePathGcHandle.Free();
            return result;
        }
        
        private unsafe Mesh[] ParseScene(Scene* pScene)
        {
            var n = pScene->MNumMeshes;
            var result = new Mesh[n];
            for (var i = 0; i < n; ++i)
            {
                var pMesh = pScene->MMeshes[i];
                if (pMesh->MVertices == null) continue;
                
                var newMesh = new Mesh();
                result[i] = newMesh;

                var vertexSize = CalculateVertexSizeAndWriteOffsets(pMesh, newMesh);
                newMesh.VertexSize = (uint) vertexSize;
                
                var verticesCount = pMesh->MNumVertices;
                newMesh.Vertices = new float[vertexSize * verticesCount];
                WriteVertexData(verticesCount, vertexSize, pMesh, newMesh.Vertices);

                newMesh.Indexes = GetIndexData(pMesh);
            }

            return result;
        }

        private unsafe int CalculateVertexSizeAndWriteOffsets(ImportMesh* pMesh, Mesh newMesh)
        {
            var vertexSize = 0;
            foreach (var importer in meshImporters)
            {
                vertexSize = importer.TryAppendSize(pMesh, newMesh, vertexSize);
            }

            return vertexSize;
        }

        private unsafe void WriteVertexData(uint verticesCount, int vertexSize, ImportMesh* pMesh, float[] vertexData)
        {
            for (var i = 0; i < verticesCount; ++i)
            {
                var offset = vertexSize * i;
                foreach (var importer in meshImporters)
                {
                    offset = importer.TryCopyTo(pMesh, i, vertexData, offset);
                }
            }
        }

        private static unsafe uint[] GetIndexData(ImportMesh* pMesh)
        {
            uint indicesCount = 0;
            for (var i = 0; i < pMesh->MNumFaces; ++i)
            {
                var pFace = pMesh->MFaces[i];
                indicesCount += pFace.MNumIndices;
            }
            
            var indices = new uint[indicesCount];
            var idx = 0;
            for (var j = 0; j < pMesh->MNumFaces; ++j)
            {
                var pFace = pMesh->MFaces[j];
                indicesCount += pFace.MNumIndices;
                for (var k = 0; k < pFace.MNumIndices; ++k)
                {
                    indices[idx] = pFace.MIndices[k];
                    idx++;
                }
            }

            return indices;
        }

        private static unsafe GCHandle MarshalToPinnedAscii(string str, out byte* pointer)
        {
            var ascii = Encoding.ASCII.GetBytes(str);
            var pinnedArray = GCHandle.Alloc(ascii, GCHandleType.Pinned);
            var intPtr = pinnedArray.AddrOfPinnedObject();
            pointer = (byte*) intPtr.ToPointer();
            return pinnedArray;
        }
    }
}