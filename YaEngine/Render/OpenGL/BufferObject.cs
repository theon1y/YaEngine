using System;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
    {
        private readonly uint handle;
        private readonly BufferTargetARB bufferType;
        private readonly GL gl;

        public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB usageHint)
        {
            this.gl = gl;
            this.bufferType = bufferType;

            handle = this.gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                this.gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, usageHint);
            }
        }

        public void Bind()
        {
            gl.BindBuffer(bufferType, handle);
        }

        public void Dispose()
        {
            gl.DeleteBuffer(handle);
        }
    }
}