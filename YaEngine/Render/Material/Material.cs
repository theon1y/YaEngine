using System.Collections.Generic;
using System.Numerics;
using YaEngine.Core;

namespace YaEngine.Render
{
    public class Material
    {
        public Shader Shader = NoShader.Value;
        public Texture Texture = BlackWhiteTexture.Value;
        
        public Dictionary<string, Vector4> Vector4Uniforms = new();
            
        public Material Copy()
        {
            return new()
            {
                Shader = Shader,
                Texture = Texture,
                Vector4Uniforms = Vector4Uniforms.Copy()
            };
        }
    }
}