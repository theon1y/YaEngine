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

        public BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType, BufferUsageARB usageHint)
        {
            this.gl = gl;
            this.bufferType = bufferType;

            handle = gl.GenBuffer();
            Bind();
            gl.BufferData<TDataType>(bufferType, data, usageHint);
        }

        public void Bind()
        {
            gl.BindBuffer(bufferType, handle);
        }

        public void Update(Span<TDataType> data)
        {
            gl.BufferSubData<TDataType>(bufferType, 0, data);
        }

        public void Dispose()
        {
            gl.DeleteBuffer(handle);
        }
    }
}