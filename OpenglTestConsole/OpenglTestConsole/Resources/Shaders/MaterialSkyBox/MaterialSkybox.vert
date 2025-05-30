
#version 330 core
layout(location = 0) in vec3 aPos;

uniform mat4 view;
uniform mat4 projection;

out vec3 TexCoord; // Texture coordinates to fragment shader

void main()
{
    TexCoord = aPos; // Pass texture coordinates

    // note that we read the multiplication from right to left
    gl_Position = (vec4(aPos, 1.0) * mat4(mat3(view)) * projection).xyww;
    // gl_Position = vec4(aPos, 1.0) * projection * view * model;
}
