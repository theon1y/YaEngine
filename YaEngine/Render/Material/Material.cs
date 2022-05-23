﻿using System.Collections.Generic;
using System.Numerics;

namespace YaEngine.Render
{
    public class Material
    {
        public IShader? Shader;
        public ITexture? Texture;
        
        public Dictionary<string, Vector4> Vector4Uniforms = new();
    }
}