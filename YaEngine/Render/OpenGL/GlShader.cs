using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class GlShader : IShader
    {
        private readonly GL gl;
        private uint handle;
        
        public GlShader(GL gl, string name, uint handle)
        {
            Name = name;
            this.gl = gl;
            this.handle = handle;
        }
        
        public string Name { get; }

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

        public void SetUniform<T>(int location, T value)
        {
            switch (value)
            {
                case int intValue:
                    gl.Uniform1(location, intValue);
                    break;
                case float floatValue:
                    gl.Uniform1(location, floatValue);
                    break;
                case Vector3 vector3Value:
                    gl.Uniform3(location, vector3Value);
                    break;
                case Vector4 vector4Value:
                    gl.Uniform4(location, vector4Value);
                    break;
                case Matrix4x4 mat4Value:
                    MarshalMatrix(gl, location, ref mat4Value);
                    break;
                case Matrix4x4[] mats4Value:
                    MarshalMatrices(gl, location, mats4Value);
                    break;
                default:
                    throw new ArgumentException($"Unsupported type of value {value}");
            }
        }

        private static unsafe void MarshalMatrix(GL gl, int location, ref Matrix4x4 matrix)
        {
            gl.UniformMatrix4(location, 1, false, (float*) Unsafe.AsPointer(ref matrix));
        }

        private static void MarshalMatrices(GL gl, int location, Matrix4x4[] matrices)
        {
            var span = MemoryMarshal.Cast<Matrix4x4, float>(matrices.AsSpan());
            gl.UniformMatrix4(location, (uint) matrices.Length, false, span);
        }
        
        public void Dispose()
        {
            gl.DeleteProgram(handle);
        }
    }
}