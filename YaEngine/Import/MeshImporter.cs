using System;
using System.Collections.Generic;
using Silk.NET.Assimp;
using Bone = YaEngine.Animation.Bone;
using ImportMesh = Silk.NET.Assimp.Mesh;
using Mesh = YaEngine.Render.Mesh;

namespace YaEngine.Import
{
    public class MeshImporter
    {
        private static readonly BoneWeight DefaultWeight = new(-1, 0f);
        
        public Mesh[] Import(string filePath)
        {
            return ImportUtils.ImportFromFile(filePath, ParseScene);
        }
        
        private unsafe Mesh[] ParseScene(IntPtr scenePointer)
        {
            var pScene = (Scene*) scenePointer;
            var n = pScene->MNumMeshes;
            var result = new Mesh[n];
            for (var i = 0; i < n; ++i)
            {
                var pMesh = pScene->MMeshes[i];
                if (pMesh->MVertices == null) continue;
                
                var newMesh = new Mesh();
                result[i] = newMesh;

                var boneMapping = ImportUtils.GetBoneMapping(pMesh);
                
                var vertexSize = CalculateVertexSizeAndWriteOffsets(pMesh, newMesh, boneMapping);
                newMesh.VertexSize = (uint) vertexSize;
                
                var verticesCount = pMesh->MNumVertices;
                newMesh.Vertices = new float[vertexSize * verticesCount];
                WriteVertexData(verticesCount, vertexSize, pMesh, newMesh.Vertices);
                
                WriteBoneData(newMesh, boneMapping.Weights);

                newMesh.Indexes = GetIndexData(pMesh);
            }

            return result;
        }

        private static unsafe int CalculateVertexSizeAndWriteOffsets(ImportMesh* pMesh, Mesh newMesh, BoneMapping boneMapping)
        {
            var vertexSize = 0;
            foreach (var importer in MeshImporterParts.MeshImporters)
            {
                vertexSize = importer.TryAppendSize(pMesh, newMesh, vertexSize);
            }

            if (boneMapping.Weights.Count <= 0) return vertexSize;
            
            newMesh.BoneIdOffset = vertexSize;
            vertexSize += Bone.MaxNesting;
            newMesh.BoneWeightOffset = vertexSize;
            vertexSize += Bone.MaxNesting;

            return vertexSize;
        }

        private static unsafe void WriteVertexData(uint verticesCount, int vertexSize, ImportMesh* pMesh, float[] vertexData)
        {
            for (var i = 0; i < verticesCount; ++i)
            {
                var offset = vertexSize * i;
                foreach (var importer in MeshImporterParts.MeshImporters)
                {
                    offset = importer.TryCopyTo(pMesh, i, vertexData, offset);
                }
            }
        }

        private static void WriteBoneData(Mesh mesh, Dictionary<uint, List<BoneWeight>> boneWeights)
        {
            var vertexData = mesh.Vertices;
            for (uint i = 0; i < vertexData.Length; ++i)
            {
                if (!boneWeights.TryGetValue(i, out var weights)) continue;

                var vertexOffset = i * mesh.VertexSize;
                for (var j = 0; j < Bone.MaxNesting; ++j)
                {
                    var boneIdOffset = vertexOffset + mesh.BoneIdOffset + j;
                    var boneWeightOffset = vertexOffset + mesh.BoneWeightOffset + j;
                    var weight = weights.Count > j ? weights[j] : DefaultWeight;
                    vertexData[boneIdOffset] = weight.BoneId;
                    vertexData[boneWeightOffset] = weight.Weight;
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
    }
}