using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public class GlTexture : ITexture
    {
        private readonly uint handle;
        private readonly GL gl;

        public string Name { get; }
        
        public GlTexture(GL gl, string name, uint handle)
        {
            Name = name;
            this.gl = gl;
            this.handle = handle;
        }
        
        public void Bind(int slot)
        {
            gl.ActiveTexture(TextureUnit.Texture0 + slot);
            gl.BindTexture(TextureTarget.Texture2D, handle);
        }
        
        public void Dispose()
        {
            gl.DeleteTexture(handle);
        }
    }
}