using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public class Texture : IDisposable
    {
        private readonly ITextureProvider provider;
        private uint handle;
        private GL gl;

        public readonly string Name;
        
        public bool IsLoaded { get; private set; }

        public Texture(string name, ITextureProvider provider)
        {
            Name = name;
            this.provider = provider;
        }

        public void Load(GL gl)
        {
            this.gl = gl;
            handle = gl.GenTexture();
            Bind();
            
            provider.Load(gl);
            IsLoaded = true;
        }
        
        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            gl.ActiveTexture(textureSlot);
            gl.BindTexture(TextureTarget.Texture2D, handle);
        }
        
        public void Dispose()
        {
            gl.DeleteTexture(handle);
            IsLoaded = false;
        }
    }
}