using System.Drawing;
using System.Runtime.CompilerServices;
using Silk.NET.OpenGL;

namespace YaEngine.Render.OpenGL
{
    public class GlRenderApi : RenderApi
    {
        internal readonly GL Gl;

        public GlRenderApi(GL gl, Color clearColor)
        {
            Gl = gl;
            ClearColor = clearColor;
        }

        public override void Clear()
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.ClearColor(ClearColor);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public override void Draw(Renderer renderer)
        {
            Gl.DrawElements(PrimitiveType.Triangles, (uint) renderer.Mesh.Indexes.Length,
                DrawElementsType.UnsignedInt, Unsafe.NullRef<uint>());
        }
    }
}