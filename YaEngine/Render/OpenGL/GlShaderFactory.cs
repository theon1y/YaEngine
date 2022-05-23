using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class GlShaderFactory : IShaderFactory
    {
        private readonly GL gl;

        public GlShaderFactory(GL gl)
        {
            this.gl = gl;
        }

        public IShader Create(string name, IShaderProvider vertexShader, IShaderProvider fragmentShader)
        {
            var handle = Load(vertexShader, fragmentShader);
            return new GlShader(gl, name, handle);
        }
        
        private uint Load(IShaderProvider vertexShader, IShaderProvider fragmentShader)
        {
            var vertex = LoadShader(gl, ShaderType.VertexShader, vertexShader);
            var fragment = LoadShader(gl, ShaderType.FragmentShader, fragmentShader);

            var handle = gl.CreateProgram();
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
            
            return handle;
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
    }
}