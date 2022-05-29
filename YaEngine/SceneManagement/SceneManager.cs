using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YaEngine.Bootstrap;

namespace YaEngine.SceneManagement
{
    public class SceneManager : ISceneManager
    {
        private readonly List<ISceneProvider> sceneProviders;
        private readonly WorldManager worldManager;
        
        public SceneManager(IEnumerable<ISceneProvider> sceneProviders, WorldManager worldManager)
        {
            this.worldManager = worldManager;
            this.sceneProviders = sceneProviders.ToList();
        }

        public List<string> GetScenes() => sceneProviders.Select(x => x.Name).ToList();

        public void LoadScene(string name)
        {
            var scene = sceneProviders.First(x => x.Name == name);
            worldManager.OnNextUpdate(() => LoadSceneAsync(scene));
        }

        public Task LoadStartAsync()
        {
            var startScene = sceneProviders.First();
            return LoadSceneAsync(startScene);
        }

        public Task LoadSceneAsync(string name)
        {
            var scene = sceneProviders.First(x => x.Name == name);
            return LoadSceneAsync(scene);
        }

        public async Task LoadSceneAsync(ISceneProvider sceneProvider)
        {
            worldManager.DisposeAsync();
            worldManager.LoadAsync(sceneProvider.GetSceneSystems());
        }
    }
}