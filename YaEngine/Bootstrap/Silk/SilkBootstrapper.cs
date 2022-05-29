using Silk.NET.Windowing;
using YaEngine.SceneManagement;

namespace YaEngine.Bootstrap
{
    public class SilkBootstrapper
    {
        private readonly IWindow window;
        private readonly SceneManager sceneManager;
        private readonly WorldManager worldManager;
        private string? startSceneName;

        public SilkBootstrapper(IWindow window, SceneManager sceneManager, WorldManager worldManager)
        {
            this.window = window;
            this.sceneManager = sceneManager;
            this.worldManager = worldManager;
            window.Load += Load;
            window.Update += Update;
            window.Render += Render;
            window.Closing += Close;
        }

        public void Run(string? sceneName = null)
        {
            startSceneName = sceneName;
            window.Run();
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(startSceneName))
            {
                sceneManager.LoadStartAsync().GetAwaiter().GetResult();
                return;
            }
            
            sceneManager.LoadSceneAsync(startSceneName).GetAwaiter().GetResult();
        }

        private void Update(double deltaTime)
        {
            worldManager.UpdateWorlds((float) deltaTime);
        }

        private void Render(double deltaTime)
        {
            worldManager.RenderWorlds((float) deltaTime);
        }

        private void Close()
        {
            worldManager.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}