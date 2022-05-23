using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Audio;
using YaEngine.Bootstrap;
using YaEngine.Core;
using YaEngine.Import;
using YaEngine.Render;

namespace YaEngine
{
    public class BuildSceneSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.Third;

        private readonly MeshImporter meshImporter;
        private readonly AnimationImporter animationImporter;
        private readonly AvatarImporter avatarImporter;
        
        public BuildSceneSystem(MeshImporter meshImporter, AnimationImporter animationImporter, AvatarImporter avatarImporter)
        {
            this.meshImporter = meshImporter;
            this.animationImporter = animationImporter;
            this.avatarImporter = avatarImporter;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            var spotlightColor = Color.White.ToVector3();

            var texturePath =
                "D:/Projects/project-v/Assets/Content/CharacterContent/Parts/Body/Textures/T_body1_base3_D.png";
            var textureName = Path.GetFileNameWithoutExtension(texturePath);
            var charTexture = new TextureInitializer(textureName, new FileTextureProvider(texturePath));
            
            var cameraEntity = world.Create(
                new Camera { Fov = 45 },
                new Transform
                {
                    Position = new Vector3(-4, 11, 15),
                    Rotation = MathUtils.FromEulerDegrees(34, 160, 0)
                });

            var lightParentTransform = new Transform
            {
                Position = new Vector3(0f, 5f, 5f),
            };
            var lightEntity = world.Create(
                new Transform
                {
                    Parent = lightParentTransform
                },
                new SpotLight { Color = spotlightColor },
                new RendererInitializer
                {
                    Material = new MaterialInitializer
                    {
                        ShaderInitializer = ColorShader.Value,
                        Vector4Uniforms = new Dictionary<string, Vector4>
                        {
                            ["uColor"] = new(spotlightColor, 1f)
                        }
                    },
                    Mesh = new Mesh
                    {
                        Vertices = new[]
                        {
                            1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
                            1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
                            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
                            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                        },
                        Indexes = new uint[]
                        {
                            0, 1, 2,
                            1, 3, 2
                        },
                        VertexSize = 5,
                        Uv0Offset = 3,
                    }
                });

            var meshPath = "D:/Projects/project-v/Assets/Content/CharacterContent/Model/Human_Model.fbx";
            var meshes = meshImporter.Import(meshPath);
            var avatar = avatarImporter.Import(meshPath);
            var animations = ImportAnimations();
            Console.WriteLine($"Imported animations: {string.Join(", ", animations.Select(x => x.Name))}");
            var animator = new Animator(animations, avatar);
            animator.Play(animations[0].Name);
            var meshEntity = world.Create(new Transform
                {
                    Position = new Vector3(0f, 0f, 5f),
                    Scale = Vector3.One * 0.05f,
                },
                new RendererInitializer
                {
                    Material = new MaterialInitializer
                    {
                        ShaderInitializer = DiffuseAnimationShader.Value,
                        TextureInitializer = charTexture
                    },
                    Mesh = meshes[0]
                },
                animator);

            var musicEntity = world.Create(new Transform(), new Music(),
                new AudioInitializer
                {
                    AudioProvider = new RiffWaveAudioProvider("D:/Projects/project-v/fmod-audio/Assets/music/abandoned_village_1_120.wav")
                });
            
            return Task.CompletedTask;
        }

        private Animation.Animation[] ImportAnimations()
        {
            var animationsPaths = new[]
            {
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Idle/idle/neutral_idle.fbx",
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Hit/hit_react.fbx",
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Moving/run_slow.fbx",
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Moving/run_slow_weapon.fbx",
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Rifle/rifle_idle_free.fbx",
                "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Rifle/rifle_shot.fbx",
            };
            return animationsPaths
                .SelectMany(x =>
                {
                    var prefix = Path.GetFileNameWithoutExtension(x);
                    var animations = animationImporter.Import(x);
                    foreach (var animation in animations)
                    {
                        animation.Name = prefix + animation.Name;
                    }
                    return animations;
                })
                .ToArray();
        }
    }
}