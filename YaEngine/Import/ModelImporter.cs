using System;
using Silk.NET.Assimp;
using ImportMesh = Silk.NET.Assimp.Mesh;

namespace YaEngine.Import
{
    public class ModelImporter
    {
        private readonly MeshImporter meshImporter;
        private readonly AnimationImporter animationImporter;
        private readonly AvatarImporter avatarImporter;

        public ModelImporter(MeshImporter meshImporter, AnimationImporter animationImporter, AvatarImporter avatarImporter)
        {
            this.meshImporter = meshImporter;
            this.animationImporter = animationImporter;
            this.avatarImporter = avatarImporter;
        }

        public ModelImporterResult Import(string filePath, ImportOptions options)
        {
            return ImportUtils.ImportFromFile(filePath, options, ParseScene);
        }
        
        private unsafe ModelImporterResult ParseScene(IntPtr scenePointer, ImportOptions options)
        {
            var pScene = (Scene*) scenePointer;
            var metadata = new AssimpMetadata(pScene->MMetaData);
            var avatar = avatarImporter.ParseScene(scenePointer, options);
            var animations = animationImporter.ParseScene(scenePointer, options);
            var meshes = meshImporter.ParseScene(scenePointer, options);

            return new ModelImporterResult
            {
                Avatar = avatar,
                Animations = animations,
                Meshes = meshes,
            };
        }
    }
}