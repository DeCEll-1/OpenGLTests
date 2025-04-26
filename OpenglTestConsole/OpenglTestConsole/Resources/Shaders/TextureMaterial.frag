
#version 330 core

in vec3 FragPos; // moved frag pos
in vec3 Normal; // normals from the cpu
in vec2 TexCoord; // tex coordinates from the cpu

struct Material {
    sampler2D texture; // tex
    vec4 colMultiplier; // color multiplier
};

uniform Material material;

out vec4 FragColor;

void main()
{
    vec4 col = texture(material.texture, TexCoord);

    // Apply lighting to the texture color
    FragColor = vec4(col * material.colMultiplier);
}
