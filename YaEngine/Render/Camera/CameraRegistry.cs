using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Render
{
    public class CameraRegistry : IComponent
    {
        public HashSet<uint> Cameras = new();
    }
}