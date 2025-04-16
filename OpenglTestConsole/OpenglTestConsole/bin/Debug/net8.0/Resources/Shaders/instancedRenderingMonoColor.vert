#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec4 aColor;

// for transformations
layout(location = 12) in mat4 model; // uses 12,13,14,15

uniform mat4 view;
uniform mat4 projection;
// uniform mat4 model;

out vec3 FragPos; // Position to fragment shader
out vec3 Normal; // Normal to fragment shader
out vec4 Color; // Texture coordinates to fragment shader

void main()
{
    FragPos = vec3(model * vec4(aPos, 1.0)); // Calculate world position
    Normal = normalize(mat3(transpose(inverse(model))) * aNormal);
    // Normal = mat3(transpose(inverse(model))) * aNormal; // Transform normal to world space

    // note that we read the multiplication from right to left
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    // gl_Position = vec4(aPos, 1.0) * projection * view * model;
}
