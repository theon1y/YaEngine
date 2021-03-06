using System.Collections.Generic;
using System.Numerics;

namespace YaEngine.Render
{
    public class MaterialInitializer
    {
        public ShaderInitializer ShaderInitializer;
        public TextureInitializer TextureInitializer;
        public Blending Blending;
        
        public Dictionary<string, Vector4> Vector4Uniforms = new();
        public Dictionary<string, TextureInitializer> TextureUniforms = new();
        public Dictionary<string, float> FloatUniforms = new();
    }
}