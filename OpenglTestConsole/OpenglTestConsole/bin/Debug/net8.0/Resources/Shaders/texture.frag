
#version 330 core

in vec2 texCoord; // tex coordinates from the cpu

uniform sampler2D texture0; // tex

out vec4 FragColor;

void main()
{
    vec4 texColor = texture(texture0, texCoord);

    FragColor = vec4(texColor);
}
