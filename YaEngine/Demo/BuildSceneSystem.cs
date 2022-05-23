using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Animation;
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
            var ambientColor = Color.LightBlue.ToVector4();
            var spotlightColor = Color.White.ToVector4();
            world.AddSingleton(new AmbientLight { Color = ambientColor });

            var texturePath =
                "D:/Projects/project-v/Assets/Content/CharacterContent/Parts/Body/Textures/T_body1_base3_D.png";
            var textureName = Path.GetFileNameWithoutExtension(texturePath);
            var charTexture = new TextureInitializer(textureName, new FileTextureProvider(texturePath));
            
            var cameraEntity = world.Create(new Camera { Fov = 110 }, new Transform());

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
                            ["uColor"] = spotlightColor
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
            
            return Task.CompletedTask;
        }

        private Animation.Animation[] ImportAnimations()
        {
            var animationsPath = "D:/Projects/project-v/Assets/Content/CharacterContent/Animation/Idle/idle/neutral_idle.fbx";
            return animationImporter.Import(animationsPath);
        }
    }
}