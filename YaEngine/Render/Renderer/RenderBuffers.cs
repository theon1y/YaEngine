using System.Collections.Generic;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Core;

namespace YaEngine.Render
{
    public class RenderBuffers : IComponent
    {
        public readonly List<RenderArguments> OpaqueRenderQueue = new(128);
        public readonly List<RenderArguments> TransparentRenderQueue = new(128);
    }

    public struct RenderArguments
    {
        public Renderer Renderer;
        public Transform RendererTransform;
    }
}