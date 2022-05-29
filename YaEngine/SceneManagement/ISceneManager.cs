using System.Collections.Generic;
using YaEcs;

namespace YaEngine.SceneManagement
{
    public interface ISceneManager : IComponent
    {
        List<string> GetScenes();
        void LoadScene(string name);
    }
}