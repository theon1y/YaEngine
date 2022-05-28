using System.Collections.Generic;
using System.Numerics;

namespace YaEngine.Render
{
    public class Material
    {
        public IShader Shader;
        public Blending Blending;
        public Dictionary<string, Vector4> Vector4Uniforms = new();
        public Dictionary<string, ITexture> TextureUniforms;
        public Dictionary<string, float> FloatUniforms = new();

        public Material(IShader shader, ITexture texture)
        {
            Shader = shader;
            TextureUniforms = new Dictionary<string, ITexture>
            {
                ["uTexture0"] = texture
            };
        }
    }
}