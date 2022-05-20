using System.IO;

namespace YaEngine.Render
{
    public readonly struct FileShaderProvider : IShaderProvider
    {
        private readonly string path;

        public FileShaderProvider(string path)
        {
            this.path = path;
        }

        public string GetSource()
        {
            return File.ReadAllText(path);
        }
    }
}