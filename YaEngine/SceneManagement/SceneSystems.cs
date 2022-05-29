using System.Collections.Generic;
using YaEcs;

namespace YaEngine.SceneManagement
{
    public class SceneSystems
    {
        public List<IInitializeSystem> InitializeSystems;
        public List<IUpdateSystem> UpdateSystems;
        public List<IDisposeSystem> DisposeSystems;
    }
}