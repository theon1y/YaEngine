namespace YaEngine.SceneManagement
{
    public interface ISceneProvider
    {
        string Name { get; }
        SceneSystems GetSceneSystems();
    }
}