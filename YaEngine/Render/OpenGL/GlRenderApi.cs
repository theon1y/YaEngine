using System;
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
            Gl.Enable(GLEnum.DepthTest);
            Gl.DepthMask(true);
            Gl.ClearColor(ClearColor);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public override void PrepareOpaqueRender()
        {
            Gl.Disable(EnableCap.Blend);
            Gl.DepthMask(true);
        }

        public override void PrepareTransparentRender()
        {
            Gl.Enable(EnableCap.Blend);
            Gl.DepthMask(false);
        }
        
        public override void Draw(Renderer renderer)
        {
            if (renderer.CullFace) Gl.Enable(EnableCap.CullFace);
            
            SetBlending(renderer.Material.Blending);
            var mode = GetRenderMode(renderer.PrimitiveType);
            if (renderer.InstanceCount > 0)
            {
                Gl.DrawElementsInstanced(mode, (uint)renderer.Mesh.Indexes.Length,
                    DrawElementsType.UnsignedInt, Unsafe.NullRef<uint>(), renderer.InstanceCount);
            }
            else
            {
                Gl.DrawElements(mode, (uint)renderer.Mesh.Indexes.Length,
                    DrawElementsType.UnsignedInt, Unsafe.NullRef<uint>());   
            }
            
            Gl.Disable(EnableCap.CullFace);
        }

        private static PrimitiveType GetRenderMode(Primitive primitive)
        {
            return primitive switch
            {
                Primitive.Triangle => PrimitiveType.Triangles,
                Primitive.Quad => PrimitiveType.Quads,
                Primitive.Line => PrimitiveType.Lines,
                Primitive.Point => PrimitiveType.Points,
                _ => throw new ArgumentOutOfRangeException(nameof(primitive), primitive, null)
            };
        }

        private void SetBlending(Blending blending)
        {
            if (blending == Blending.Disabled) return;
            
            switch (blending)
            {
                case Blending.Multiply:
                    Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;
                case Blending.Additive:
                    Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blending), blending, null);
            }
        }
    }
}