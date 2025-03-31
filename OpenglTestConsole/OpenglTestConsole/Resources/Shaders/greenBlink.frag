#version 330 core
out vec4 FragColor;
uniform float t;
void main()
{
    FragColor = vec4(vec3(0., abs(sin(t)), 0.), 1.0f);
}
