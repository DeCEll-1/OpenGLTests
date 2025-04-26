#version 330 core

in vec3 FragPos; // moved frag pos
in vec3 Normal; // normals from the cpu

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};
uniform Material material;

struct Light {
    vec3 position;
    vec3 color;
};
uniform Light light;

uniform vec3 viewPos; // camera

out vec4 FragColor;

void main()
{
    // ambient
    vec3 ambient = light.color * material.ambient;

    // diffuse
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.color * (diff * material.diffuse);

    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    // setted reflectDir to minus because the specular was at the opposide side it shouldve been
    float spec = pow(max(dot(viewDir, -reflectDir), 0.0), material.shininess);
    vec3 specular = light.color * (spec * material.specular);

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}
