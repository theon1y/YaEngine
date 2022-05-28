using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public static class ImportUtils
    {
        public static unsafe T ImportFromFile<T>(string filePath, ImportOptions options, Func<IntPtr, ImportOptions, T> importer)
        {
            var fullPath = System.IO.Path.GetFullPath(filePath);
            var filePathGcHandle = MarshalToPinnedAscii(fullPath, out var filePathPointer);
            var assimp = Assimp.GetApi();
            var scenePointer = assimp.ImportFile(filePathPointer, 0);
            if (scenePointer == null)
            {
                var errorPointer = assimp.GetErrorString();
                var error = Marshal.PtrToStringUTF8(new IntPtr(errorPointer));
                throw new Exception($"Could not import {System.IO.Path.GetFileNameWithoutExtension(fullPath)}:\n{error}");
            }

            var result = importer((IntPtr)scenePointer, options);
            
            assimp.ReleaseImport(scenePointer);
            filePathGcHandle.Free();
            return result;
        }
        
        public static unsafe GCHandle MarshalToPinnedAscii(string str, out byte* pointer)
        {
            var ascii = Encoding.ASCII.GetBytes(str);
            var pinnedArray = GCHandle.Alloc(ascii, GCHandleType.Pinned);
            var intPtr = pinnedArray.AddrOfPinnedObject();
            pointer = (byte*) intPtr.ToPointer();
            return pinnedArray;
        }

        public static unsafe BoneMapping GetBoneMapping(Mesh* pMesh)
        {
            var boneCount = (int) pMesh->MNumBones;
            var bones = new Dictionary<string, Animation.Bone>(boneCount);
            var weights = new Dictionary<uint, List<BoneWeight>>((int)pMesh->MNumVertices);
            for (var i = 0; i < boneCount; ++i)
            {
                var bone = pMesh->MBones[i];
                string name = bone->MName.ToNormalizedString();
                if (!bones.TryGetValue(name, out var extractedBone))
                {
                    extractedBone = new Animation.Bone(bones.Count, Matrix4x4.Transpose(bone->MOffsetMatrix));
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

        public static string ToNormalizedString(this AssimpString str)
        {
            return str.AsString
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace("-", "_");
        }
    }
}