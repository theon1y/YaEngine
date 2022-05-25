namespace YaEngine.VFX.ParticleSystem.Shaders
{
    public class ParticleShader
    {
        public static string GetVertexShaderWithBillboard(string billboardFunction)
        {
            return Vertex.Replace("BILLBOARD_FUNCTION", billboardFunction);
        }
        public const string Vertex = @"
#version 330 core
in vec3 vPos;
in vec2 vUv;
in vec3 particlePosition;
in vec4 particleRotation;
in vec3 particleScale;
in vec4 particleColor;
in vec2 particleUv;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec2 fUv;
out vec4 fColor;

BILLBOARD_FUNCTION

mat4 scale(vec3 v)
{
	return mat4(v.x, 0, 0, 0,
	            0, v.y, 0, 0,
	            0, 0, v.z, 0,
	            0, 0, 0, 1);
}

mat4 rotate(vec4 q)
{
    mat4 result = mat4(1.0f);

    float xx = q.x * q.x;
    float yy = q.y * q.y;
    float zz = q.z * q.z;

    float xy = q.x * q.y;
    float wz = q.z * q.w;
    float xz = q.z * q.x;
    float wy = q.y * q.w;
    float yz = q.y * q.z;
    float wx = q.x * q.w;

    result[0][0] = 1.0f - 2.0f * (yy + zz);
    result[0][1] = 2.0f * (xy + wz);
    result[0][2] = 2.0f * (xz - wy);

    result[1][0] = 2.0f * (xy - wz);
    result[1][1] = 1.0f - 2.0f * (zz + xx);
    result[1][2] = 2.0f * (yz + wx);

    result[2][0] = 2.0f * (xz + wy);
    result[2][1] = 2.0f * (yz - wx);
    result[2][2] = 1.0f - 2.0f * (yy + xx);

    return result;
}

void main()
{
    mat4 viewModel = uView * uModel;

    mat4 particleTransform = rotate(particleRotation) * scale(particleScale);
    mat4 billboard = billboard(viewModel);
    //mat4 billboard = viewModel;
    mat4 worldTransform = uProjection * billboard;
    gl_Position = worldTransform * particleTransform * (vec4(vPos + particlePosition, 1.0f));

    fColor = particleColor;
    fUv = vUv + particleUv;
}
";

        public const string Fragment = @"
#version 330 core

in vec2 fUv;
in vec4 fColor;

uniform sampler2D uTexture0;

out vec4 FragColor;

void main()
{
      vec4 textureColor = texture(uTexture0, fUv);

      FragColor = fColor * textureColor;
}
";
    }
}