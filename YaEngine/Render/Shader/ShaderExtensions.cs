using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public static class ShaderExtensions
    {
        public static int GetUniformLocation(this Shader shader, string name)
        {
            if (shader.TryGetUniformLocation(name, out var location)) return location;
            
            throw new Exception($"Could not find uniform {name} on shader {shader.Name}.");
        }
        
        public static bool TrySetUniform(this Shader shader, GL gl, string name, int value)
        {
            if (!shader.TryGetUniformLocation(name, out var location)) return false;

            gl.Uniform1(location, value);
            return true;
        }
        
        public static bool TrySetUniform(this Shader shader, GL gl, string name, float value)
        {
            if (!shader.TryGetUniformLocation(name, out var location)) return false;

            gl.Uniform1(location, value);
            return true;
        }
        
        public static bool TrySetUniform(this Shader shader, GL gl, string name, Vector3 value)
        {
            if (!shader.TryGetUniformLocation(name, out var location)) return false;
            
            gl.Uniform3(location, value);
            return true;
        }
        
        public static bool TrySetUniform(this Shader shader, GL gl, string name, Vector4 value)
        {
            if (!shader.TryGetUniformLocation(name, out var location)) return false;

            gl.Uniform4(location, value);
            return true;
        }
        
        public static bool TrySetUniform(this Shader shader, GL gl, string name, Matrix4x4 value)
        {
            unsafe
            {
                if (!shader.TryGetUniformLocation(name, out var location)) return false;

                gl.UniformMatrix4(location, 1, false, (float*) &value);
                return true;
            }
        }
        
        public static void SetUniform(this Shader shader, GL gl, string name, int value)
        {
            var location = shader.GetUniformLocation(name);
            gl.Uniform1(location, value);
        }
        
        public static void SetUniform(this Shader shader, GL gl, string name, float value)
        {
            var location = shader.GetUniformLocation(name);
            gl.Uniform1(location, value);
        }
        
        public static void SetUniform(this Shader shader, GL gl, string name, Vector3 value)
        {
            var location = shader.GetUniformLocation(name);
            gl.Uniform3(location, value);
        }
        
        public static void SetUniform(this Shader shader, GL gl, string name, Vector4 value)
        {
            var location = shader.GetUniformLocation(name);
            gl.Uniform4(location, value);
        }
        
        public static void SetUniform(this Shader shader, GL gl, string name, Matrix4x4 value)
        {
            unsafe
            {
                var location = shader.GetUniformLocation(name);
                gl.UniformMatrix4(location, 1, false, (float*) &value);
            }
        }
    }
}