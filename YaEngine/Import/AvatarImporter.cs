using System;
using System.Collections.Generic;
using System.Numerics;
using Silk.NET.Assimp;
using YaEngine.Animation;
using ImportMesh = Silk.NET.Assimp.Mesh;
using ImportAnimation = Silk.NET.Assimp.Animation;

namespace YaEngine.Import
{
    public class AvatarImporter
    {
        public Avatar Import(string filePath, ImportOptions options)
        {
            return ImportUtils.ImportFromFile(filePath, options, ParseScene);
        }
        
        public unsafe Avatar ParseScene(IntPtr scenePointer, ImportOptions options)
        {
            var pScene = (Scene*) scenePointer;
            var metadata = new AssimpMetadata(pScene->MMetaData);
            pScene->MRootNode->MTransformation = metadata.GetValidRootTransformation(options.Scale);
            var hierarchy = ParseHierarchyRecursive(pScene->MRootNode, new List<AvatarNode>(),
                -1);
            var avatar = new Avatar { Hierarchy = hierarchy.ToArray() };

            var meshesCount = pScene->MNumMeshes;
            for (var i = 0; i < meshesCount; ++i)
            {
                var boneMapping = ImportUtils.GetBoneMapping(pScene->MMeshes[i]);
                avatar.Bones = boneMapping.Bones;
            }
            
            return avatar;
        }

        private static unsafe List<AvatarNode> ParseHierarchyRecursive(Node* pNode, List<AvatarNode> nodes, int parentIndex)
        {
            var meshIndexes = new uint[pNode->MNumMeshes];
            for (var i = 0; i < meshIndexes.Length; ++i)
            {
                meshIndexes[i] = pNode->MMeshes[i];
            }

            var localTransform = Matrix4x4.Transpose(pNode->MTransformation);
            var node = new AvatarNode(pNode->MName.ToNormalizedString(), parentIndex, meshIndexes, localTransform);
            parentIndex = nodes.Count;
            nodes.Add(node);
            
            var childCount = pNode->MNumChildren;
            for (var i = 0; i < childCount; ++i)
            {
                ParseHierarchyRecursive(pNode->MChildren[i], nodes, parentIndex);
            }

            return nodes;
        }
    }
}