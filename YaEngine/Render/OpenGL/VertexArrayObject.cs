using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class VertexArrayObject<TVertexType> : IDisposable
        where TVertexType : unmanaged
    {
        private readonly uint handle;
        private readonly GL gl;

        public VertexArrayObject(GL gl)
        {
            this.gl = gl;

            handle = gl.GenVertexArray();
            Bind();
        }

        public unsafe void VertexAttributePointer(uint index, int size, VertexAttribPointerType type, uint vertexSize,
            int offSet, uint divisor)
        {
            gl.EnableVertexAttribArray(index);
            gl.VertexAttribPointer(index, size, type, false, vertexSize * (uint) sizeof(TVertexType),
                (void*) (offSet * sizeof(TVertexType)));
            gl.VertexAttribDivisor(index, divisor);
        }

        public void Bind()
        {
            gl.BindVertexArray(handle);
        }

        public void Dispose()
        {
            gl.DeleteVertexArray(handle);
        }
    }
}