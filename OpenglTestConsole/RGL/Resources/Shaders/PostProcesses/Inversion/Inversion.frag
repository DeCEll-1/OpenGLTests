
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorBuffer;
uniform sampler2D depthBuffer;

void main()
{
    FragColor = vec4(vec3(1.0 - texture(colorBuffer, TexCoords).rgb), 1.0);
}
