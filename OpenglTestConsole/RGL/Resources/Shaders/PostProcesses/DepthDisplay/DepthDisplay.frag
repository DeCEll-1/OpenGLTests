
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorBuffer;
uniform sampler2D depthBuffer;

uniform vec3 fogColor;

uniform float near; // Camera near plane
uniform float far; // Camera far plane
uniform float fogDensity; // For exponential fog
uniform float fogStart; // the start of the fog

// Convert depth buffer value to linear depth
float LinearizeDepth(float depth)
{
    float ndc = depth * 2.0 - 1.0;
    return (2.0 * near * far) / (far + near - ndc * (far - near));
}

void main()
{
    vec3 color = texture(colorBuffer, TexCoords).rgb;
    float depth = texture(depthBuffer, TexCoords).r;

    float linearDepth = LinearizeDepth(depth) - fogStart;

    float fogFactor = exp(-fogDensity * linearDepth);
    fogFactor = clamp(fogFactor, 0.0, 1.0);

    vec3 finalColor = mix(fogColor, color, fogFactor);
    FragColor = vec4(finalColor, 1.0);
}
