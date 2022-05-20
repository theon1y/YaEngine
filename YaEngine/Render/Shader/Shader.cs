using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public class Shader : IDisposable
    {
        private readonly IShaderProvider vertexShaderProvider;
        private readonly IShaderProvider fragmentShaderProvider;
        private uint handle;
        private GL gl;
        
        public readonly string Name;
        public bool IsLoaded;
        
        public Shader(string name, IShaderProvider vertexShaderProvider, IShaderProvider fragmentShaderProvider)
        {
            Name = name;
            this.vertexShaderProvider = vertexShaderProvider;
            this.fragmentShaderProvider = fragmentShaderProvider;
        }

        public void Load(GL gl)
        {
            this.gl = gl;
            var vertex = LoadShader(gl, ShaderType.VertexShader, vertexShaderProvider);
            var fragment = LoadShader(gl, ShaderType.FragmentShader, fragmentShaderProvider);

            handle = gl.CreateProgram();
            gl.AttachShader(handle, vertex);
            gl.AttachShader(handle, fragment);
            gl.LinkProgram(handle);
            
            gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = gl.GetProgramInfoLog(handle);
                throw new Exception($"Could not link program: {log}");
            }
            
            gl.DetachShader(handle, vertex);
            gl.DetachShader(handle, fragment);
            gl.DeleteShader(vertex);
            gl.DeleteShader(fragment);
            IsLoaded = true;
        }

        public void Use()
        {
            gl.UseProgram(handle);
        }

        public bool TryGetUniformLocation(string name, out int location)
        {
            location = gl.GetUniformLocation(handle, name);
            return location != -1;
        }

        public bool TryGetAttributeLocation(string name, out int location)
        {
            location = gl.GetAttribLocation(handle, name);
            return location != -1;
        }

        private static uint LoadShader(GL gl, ShaderType shaderType, IShaderProvider provider)
        {
            var source = provider.GetSource();
            var handle = gl.CreateShader(shaderType);
            gl.ShaderSource(handle, source);
            gl.CompileShader(handle);
            var log = gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(log))
            {
                throw new Exception($"Could not compile {shaderType}:\n{log}");
            }

            return handle;
        }
        
        public void Dispose()
        {
            gl.DeleteProgram(handle);
            IsLoaded = false;
        }
    }
}