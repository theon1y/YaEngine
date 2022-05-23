namespace YaEngine.Render
{
    public record ShaderInitializer(string Name, IShaderProvider VertexShader, IShaderProvider FragmentShader);
}