using YaEngine.Render;

namespace YaEngine
{
    public class DiffuseShader
    {
        public static readonly Shader Value = new("DiffuseShader",
            new StringShaderProvider(Vertex),
            new StringShaderProvider(Fragment));
        
        private const string Vertex = @"
#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 2) in vec2 vUv;
layout (location = 4) in vec3 vNormal;
layout (location = 5) in vec4 vBoneWeights;
layout (location = 6) in vec4 vBoneIds;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

const int MAX_BONES = 64;
const int MAX_NESTING = 4;
uniform mat4 uFinalBoneMatrices[MAX_BONES];

out vec3 fPos;
out vec2 fUv;
out vec3 fNormal;
out vec4 fColor;

void main()
{
    mat4 boneTransform = mat4(0.0f);
    for(int i = 0 ; i < MAX_NESTING ; i++)
    {
        int id = int(vBoneIds[i]);
        boneTransform += uFinalBoneMatrices[id] * vBoneWeights[i];
    }

    vec4 totalPosition = boneTransform * vec4(vPos,1.0f);
    vec3 totalNormal = mat3(boneTransform) * vNormal;

    gl_Position = uProjection * uView * uModel * totalPosition;
    fNormal = mat3(transpose(inverse(uModel))) * totalNormal;

    fPos = vec3(uModel * totalPosition);
    fUv = vUv;
}
";

        private const string Fragment = @"
#version 330 core

in vec2 fUv;
in vec3 fNormal;
in vec3 fPos;
in vec4 fColor;

uniform sampler2D uTexture0;
uniform vec3 lightColor;
uniform vec3 lightPos;

out vec4 FragColor;

void main()
{
      float ambientStrength = 0.1;
      vec3 ambient = ambientStrength * lightColor;

      vec3 norm = normalize(fNormal);
      vec3 lightDirection = normalize(lightPos - fPos);
      float diff = max(dot(norm, lightDirection), 0.0);
      vec3 diffuse = diff * lightColor;

      vec4 color = vec4(0.0,1.0,1.0,1.0);
      vec4 objectColor = texture(uTexture0, fUv);
      vec4 result = vec4(ambient + diffuse, 1.0) * objectColor;

      FragColor = result;
}
";
    }
}