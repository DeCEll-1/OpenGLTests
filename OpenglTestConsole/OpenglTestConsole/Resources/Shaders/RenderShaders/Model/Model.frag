#version 330 core

in vec3 FragPos; // moved frag pos
in vec3 Normal; // normals from the cpu
in vec2 TexCoord; // tex coordinates from the cpu

uniform sampler2D color;
uniform sampler2D occlusion;
uniform sampler2D normal; // add this l8r

out vec4 FragColor;

void main()
{
    vec4 col = texture(color, TexCoord);
    float occ = texture(occlusion, TexCoord).r;

    vec3 res = vec3(col * occ);

    // Apply lighting to the texture color
    FragColor = vec4(res, 1.);
}
