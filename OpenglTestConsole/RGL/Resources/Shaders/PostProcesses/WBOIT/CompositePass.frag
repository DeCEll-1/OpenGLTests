in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D colorBuffer;
uniform sampler2D depthBuffer;

uniform sampler2D uAccumTex; // GL_COLOR_ATTACHMENT0
uniform sampler2D uRevealTex; // GL_COLOR_ATTACHMENT1

// epsilon number
const float EPSILON = 0.00001f;

// calculate floating point numbers equality accurately
bool isApproximatelyEqual(float a, float b)
{
    return abs(a - b) <= (abs(a) < abs(b) ? abs(b) : abs(a)) * EPSILON;
}

// get the max value between three values
float max3(vec3 v)
{
    return max(max(v.x, v.y), v.z);
}

void main()
{
    // fragment revealage
    float revealage = texture(uRevealTex, TexCoords).r;

    // save the blending and color texture fetch cost if there is not a transparent fragment
    if (isApproximatelyEqual(revealage, 1.0f))
    {
        FragColor = texture(colorBuffer, TexCoords);
        return;
    }

    vec4 accumulation = texture(uAccumTex, TexCoords);

    // suppress overflow
    if (isinf(max3(abs(accumulation.rgb))))
        accumulation.rgb = vec3(accumulation.a);

    // prevent floating point precision bug
    vec3 average_color = accumulation.rgb / max(accumulation.a, EPSILON);

    // blend pixels
    FragColor = vec4(average_color, 1.0f - revealage);
}
