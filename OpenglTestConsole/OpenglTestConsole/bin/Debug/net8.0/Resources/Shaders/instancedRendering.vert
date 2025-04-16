#version 330 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

// for transformations
layout(location = 7) in mat4 model; // uses 7,8,9,10
layout(location = 11) in mat4 view; // uses 11,12,13,14
layout(location = 15) in mat4 projection; // uses 15,16,17,18

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}
