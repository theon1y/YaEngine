using System;
using System.Collections.Generic;
using Silk.NET.Assimp;
using YaEngine.Render;
using Bone = YaEngine.Animation.Bone;
using ImportMesh = Silk.NET.Assimp.Mesh;
using Mesh = YaEngine.Render.Mesh;

namespace YaEngine.Import
{
    public class MeshImporter
    {
        private static readonly BoneWeight DefaultWeight = new(-1, 0f);
        
        public Mesh[] Import(string filePath, ImportOptions options)
        {
            return ImportUtils.ImportFromFile(filePath, options, ParseScene);
        }

        public unsafe Mesh[] ParseScene(IntPtr scenePointer, ImportOptions options)
        {
            var pScene = (Scene*) scenePointer;
            var n = pScene->MNumMeshes;
            var result = new Mesh[n];
            for (var i = 0; i < n; ++i)
            {
                var pMesh = pScene->MMeshes[i];
                if (pMesh->MVertices == null) continue;

                var newMesh = new Mesh { Name = pMesh->MName.ToNormalizedString() };
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
                if (!importer.HasAttribute(pMesh)) continue;
                
                importer.AddAttribute(newMesh, vertexSize);
                vertexSize += importer.Size;
            }

            if (boneMapping.Weights.Count <= 0) return vertexSize;

            var idAttribute = MeshAttributes.BoneIds;
            idAttribute.Offset = vertexSize;
            newMesh.Attributes.Add(idAttribute);
            vertexSize += Bone.MaxNesting;

            var weightAttribute = MeshAttributes.BoneWeights;
            weightAttribute.Offset = vertexSize;
            newMesh.Attributes.Add(weightAttribute);
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
                    if (!importer.HasAttribute(pMesh)) continue;
                    
                    importer.CopyTo(pMesh, i, vertexData, offset);
                    offset += importer.Size;
                }
            }
        }

        private static void WriteBoneData(Mesh mesh, Dictionary<uint, List<BoneWeight>> boneWeights)
        {
            var vertexData = mesh.Vertices;
            var idAttribute = mesh.Attributes.Find(x => x.Name == MeshAttributes.BoneIds.Name);
            var weightAttribute = mesh.Attributes.Find(x => x.Name == MeshAttributes.BoneWeights.Name);
            for (uint i = 0; i < vertexData.Length; ++i)
            {
                if (!boneWeights.TryGetValue(i, out var weights)) continue;

                var vertexOffset = i * mesh.VertexSize;
                for (var j = 0; j < Bone.MaxNesting; ++j)
                {
                    var boneIdOffset = vertexOffset + idAttribute.Offset + j;
                    var boneWeightOffset = vertexOffset + weightAttribute.Offset + j;
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