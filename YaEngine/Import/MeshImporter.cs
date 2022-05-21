using System;
using System.Collections.Generic;
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

                var boneMapping = GetBoneMapping(pMesh);
                
                var vertexSize = CalculateVertexSizeAndWriteOffsets(pMesh, newMesh, boneMapping);
                newMesh.VertexSize = (uint) vertexSize;
                
                var verticesCount = pMesh->MNumVertices;
                newMesh.Vertices = new float[vertexSize * verticesCount];
                WriteVertexData(verticesCount, vertexSize, pMesh, newMesh.Vertices);
                
                newMesh.Bones = boneMapping.Bones;
                WriteBoneData(newMesh, boneMapping.Weights);

                newMesh.Indexes = GetIndexData(pMesh);
            }

            return result;
        }

        private unsafe int CalculateVertexSizeAndWriteOffsets(ImportMesh* pMesh, Mesh newMesh, BoneMapping boneMapping)
        {
            var vertexSize = 0;
            foreach (var importer in meshImporters)
            {
                vertexSize = importer.TryAppendSize(pMesh, newMesh, vertexSize);
            }

            if (boneMapping.Weights.Count <= 0) return vertexSize;
            
            newMesh.BoneIdOffset = vertexSize;
            vertexSize += Animation.Bone.MaxNesting;
            newMesh.BoneWeightOffset = vertexSize;
            vertexSize += Animation.Bone.MaxNesting;

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

        private static void WriteBoneData(Mesh mesh, Dictionary<uint, List<BoneWeight>> boneWeights)
        {
            var vertexData = mesh.Vertices;
            for (uint i = 0; i < vertexData.Length; ++i)
            {
                if (!boneWeights.TryGetValue(i, out var weights)) continue;

                var vertexOffset = i * mesh.VertexSize;
                for (var j = 0; j < weights.Count && j < Animation.Bone.MaxNesting; ++j)
                {
                    var weight = weights[j];
                    var boneIdOffset = vertexOffset + mesh.BoneIdOffset + j;
                    var boneWeightOffset = vertexOffset + mesh.BoneWeightOffset + j;
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

        private static unsafe BoneMapping GetBoneMapping(ImportMesh* pMesh)
        {
            var boneCount = (int) pMesh->MNumBones;
            var bones = new Dictionary<string, Animation.Bone>(boneCount);
            var weights = new Dictionary<uint, List<BoneWeight>>((int)pMesh->MNumVertices);
            for (var i = 0; i < boneCount; ++i)
            {
                var bone = pMesh->MBones[i];
                string name = bone->MName;
                if (!bones.TryGetValue(name, out var extractedBone))
                {
                    extractedBone = new Animation.Bone(bones.Count, bone->MOffsetMatrix);
                    bones.Add(name, extractedBone);
                }
                
                var id = extractedBone.Id;
                var boneWeights = bone->MWeights;
                var weightsCount = bone->MNumWeights;
                for (var j = 0; j < weightsCount; ++j)
                {
                    var vertexWeight = boneWeights[j];
                    if (!weights.TryGetValue(vertexWeight.MVertexId, out var vertexWeights))
                    {
                        vertexWeights = new List<BoneWeight>(Animation.Bone.MaxNesting);
                        weights[vertexWeight.MVertexId] = vertexWeights;
                    }
                    vertexWeights.Add(new BoneWeight(id, vertexWeight.MWeight));
                }
            }

            return new BoneMapping(bones, weights);
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