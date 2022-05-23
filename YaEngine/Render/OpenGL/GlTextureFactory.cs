using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class GlTextureFactory : ITextureFactory
    {
        private readonly GL gl;
        
        public GlTextureFactory(GL gl)
        {
            this.gl = gl;
        }

        public ITexture Create(string name, TextureProvider provider)
        {
            var handle = gl.GenTexture();
            var texture = new GlTexture(gl, name, handle);
            texture.Bind(0);
            
            var type = GetGlTarget(provider);
            var format = GetGlFormat(provider);
            var sourceFormat = GetGlSourceFormat(provider);
            var sourcePixelType = GetGlSourcePixelType(provider);
            unsafe
            {
                var data = provider.Load();
                fixed (void* d = &data[0])
                {
                    gl.TexImage2D(type, 0, format, provider.Width, provider.Height, 0,
                        sourceFormat, sourcePixelType, d);
                }
            }
            SetParameters(gl, provider);

            return texture;
        }

        private static void SetParameters(GL gl, TextureProvider provider)
        {
            var target = GetGlTarget(provider);
            var wrap = GetGlWrap(provider);
            var filter = GetGlFilter(provider);
            gl.TexParameter(target, TextureParameterName.TextureWrapS, wrap);
            gl.TexParameter(target, TextureParameterName.TextureWrapT, wrap);
            gl.TexParameter(target, TextureParameterName.TextureMinFilter, filter);
            gl.TexParameter(target, TextureParameterName.TextureMagFilter, filter);
            if (provider.UseMipMaps)
            {
                gl.GenerateMipmap(target);
            }
        }
        
        private static int GetGlFilter(TextureProvider provider)
        {
            return provider.Filter switch
            {
                TextureFilter.Linear => (int) GLEnum.Linear,
                TextureFilter.Nearest => (int) GLEnum.Nearest,
                _ => throw new ArgumentException($"Unsupported filter type {provider.Filter}")
            };
        }

        private static int GetGlWrap(TextureProvider provider)
        {
            return provider.Wrap switch
            {
                TextureWrap.Repeat => (int) GLEnum.Repeat,
                TextureWrap.MirroredRepeat => (int) GLEnum.MirroredRepeat,
                TextureWrap.ClampToEdge => (int) GLEnum.ClampToEdge,
                _ => throw new ArgumentException($"Unsupported wrap type {provider.Wrap}")
            };
        }

        private static TextureTarget GetGlTarget(TextureProvider provider)
        {
            return provider.Type switch
            {
                TextureType.Texture2d => TextureTarget.Texture2D,
                _ => throw new ArgumentException($"Unsupported texture type {provider.Type}")
            };
        }
        
        private static int GetGlFormat(TextureProvider provider)
        {
            return provider.Format switch
            {
                TextureFormat.Rgb => (int) InternalFormat.Rgb,
                TextureFormat.Rgba => (int) InternalFormat.Rgba,
                _ => throw new ArgumentException($"Unsupported format {provider.Format}")
            };
        }
        
        private static Silk.NET.OpenGL.PixelFormat GetGlSourceFormat(TextureProvider provider)
        {
            return provider.SourceFormat switch
            {
                PixelFormat.Rgb => Silk.NET.OpenGL.PixelFormat.Rgb,
                PixelFormat.Rgba =>Silk.NET.OpenGL.PixelFormat.Rgba,
                _ => throw new ArgumentException($"Unsupported format {provider.SourceFormat}")
            };
        }

        private static Silk.NET.OpenGL.PixelType GetGlSourcePixelType(TextureProvider provider)
        {
            return provider.SourcePixelType switch
            {
                PixelType.UnsignedByte => Silk.NET.OpenGL.PixelType.UnsignedByte,
                _ => throw new ArgumentException($"Unsupported pixel type {provider.SourcePixelType}")
            };
        }
    }
}