namespace YaEngine.Render
{
    public class StringShaderProvider : IShaderProvider
    {
        private readonly string source;

        public StringShaderProvider(string source)
        {
            this.source = source;
        }

        public string GetSource() => source;
    }
}