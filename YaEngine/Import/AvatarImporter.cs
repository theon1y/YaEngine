using System;
using System.Collections.Generic;
using Silk.NET.Assimp;
using YaEngine.Animation;
using ImportMesh = Silk.NET.Assimp.Mesh;
using ImportAnimation = Silk.NET.Assimp.Animation;

namespace YaEngine.Import
{
    public class AvatarImporter
    {
        public Avatar Import(string filePath)
        {
            return ImportUtils.ImportFromFile(filePath, ParseScene);
        }
        
        private unsafe Avatar ParseScene(IntPtr scenePointer)
        {
            var pScene = (Scene*) scenePointer;
            var hierarchy = ParseHierarchyRecursive(pScene->MRootNode, new List<AvatarNode>(), -1);
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
            var node = new AvatarNode(pNode->MName, parentIndex);
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