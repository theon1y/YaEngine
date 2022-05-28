using System;
using System.Collections.Generic;
using Silk.NET.Assimp;
using YaEngine.Animation;
using ImportMesh = Silk.NET.Assimp.Mesh;
using ImportAnimation = Silk.NET.Assimp.Animation;

namespace YaEngine.Import
{
    public class AnimationImporter
    {
        private readonly IAnimationImporterPart[] importerParts = AnimationImporterParts.ImporterParts;

        public Animation.Animation[] Import(string filePath, ImportOptions options)
        {
            return ImportUtils.ImportFromFile(filePath, options, ParseScene);
        }

        public unsafe Animation.Animation[] ParseScene(IntPtr scenePointer, ImportOptions options)
        {
            var pScene = (Scene*) scenePointer;
            var n = pScene->MNumAnimations;
            var animations = new Animation.Animation[n];
            for (var i = 0; i < n; ++i)
            {
                var pAnimation = pScene->MAnimations[i];
                var newAnimation = new Animation.Animation();
                animations[i] = newAnimation;

                newAnimation.Name = pAnimation->MName.ToNormalizedString();
                newAnimation.Duration = (float) pAnimation->MDuration;
                newAnimation.FramesPerSecond = (int) pAnimation->MTicksPerSecond;

                var channelCount = pAnimation->MNumChannels;
                var boneAnimations = new Dictionary<string, BoneAnimation>((int)channelCount);
                for (var j = 0; j < channelCount; ++j)
                {
                    var channel = pAnimation->MChannels[j];
                    string boneName = channel->MNodeName.ToNormalizedString();
                    
                    var boneAnimation = new BoneAnimation { Name = boneName };
                    foreach (var importerPart in importerParts)
                    {
                        importerPart.Import(boneAnimation, channel);
                    }
                    boneAnimations.Add(boneName, boneAnimation);
                }

                newAnimation.BoneAnimations = boneAnimations;
            }

            return animations;
        }
    }
}