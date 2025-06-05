#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorBuffer;
uniform sampler2D depthBuffer;

uniform float gamma;

void main()
{
    vec3 col = texture(colorBuffer, TexCoords).rgb;

    col = pow(col, vec3(1.0 / gamma));

    FragColor = vec4(col, 1.0);
}
