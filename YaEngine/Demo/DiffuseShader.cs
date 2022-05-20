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

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec3 fPos;
out vec2 fUv;
out vec3 fNormal;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
    fPos = vec3(uModel * vec4(vPos, 1.0));
    fUv = vUv;
    fNormal = mat3(transpose(inverse(uModel))) * vNormal;
}
";

        private const string Fragment = @"
#version 330 core

in vec2 fUv;
in vec3 fNormal;
in vec3 fPos;

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