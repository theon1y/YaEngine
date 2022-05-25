using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem.Shaders
{
    public class BillboardParticleShader
    {
        public static readonly ShaderInitializer Value = new(nameof(BillboardParticleShader),
            new StringShaderProvider(
                ParticleShader.GetVertexShaderWithBillboard(BillboardFunction)),
            new StringShaderProvider(ParticleShader.Fragment));

        private const string BillboardFunction = @"
mat4 billboard(mat4 modelView)
{
    modelView[0][0] = 1.0;
    modelView[0][1] = 0.0;
    modelView[0][2] = 0.0;

    modelView[1][0] = 0.0;
    modelView[1][1] = 1.0;
    modelView[1][2] = 0.0;

    modelView[2][0] = 0.0;
    modelView[2][1] = 0.0;
    modelView[2][2] = 1.0;

    return modelView;
}
";
    }
}