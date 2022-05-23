using System;

namespace YaEngine.Render
{
    public static class ShaderExtensions
    {
        public static IShader Create(this IShaderFactory factory, ShaderInitializer initializer)
        {
            return factory.Create(initializer.Name, initializer.VertexShader, initializer.FragmentShader);
        }
        
        public static int GetUniformLocation(this IShader shader, string name)
        {
            if (shader.TryGetUniformLocation(name, out var location)) return location;
            
            throw new Exception($"Could not find uniform {name} on shader {shader.Name}.");
        }
        
        public static bool TrySetUniform<T>(this IShader shader, string name, T value)
        {
            if (!shader.TryGetUniformLocation(name, out var location)) return false;

            shader.SetUniform(location, value);
            return true;
        }
        
        public static void SetUniform<T>(this IShader shader, string name, T value)
        {
            var location = shader.GetUniformLocation(name);
            shader.SetUniform(location, value);
        }
    }
}