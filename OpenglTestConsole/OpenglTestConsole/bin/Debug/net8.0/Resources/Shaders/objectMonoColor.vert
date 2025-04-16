#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;

out vec3 FragPos; // Position to fragment shader
out vec3 Normal; // Normal to fragment shader

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    FragPos = vec3(model * vec4(aPos, 1.0)); // Calculate world position
    Normal = normalize(mat3(transpose(inverse(model))) * aNormal);
    // Normal = mat3(transpose(inverse(model))) * aNormal; // Transform normal to world space

    // note that we read the multiplication from right to left
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    // gl_Position = vec4(aPos, 1.0) * projection * view * model;
}
