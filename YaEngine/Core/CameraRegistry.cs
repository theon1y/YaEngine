using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Core
{
    public class CameraRegistry : IComponent
    {
        public HashSet<uint> Cameras = new();
    }
}