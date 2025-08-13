in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

struct Material {
    sampler2D matTexture;
    vec4 colMultiplier;
};

uniform Material material;

#ifdef TRANSPARENT
layout(location = 0) out vec4 outAccum; // accumulation buffer (RGBA)
layout(location = 1) out float outRevealage; // revealage buffer (R)

uniform sampler2D accumBuffer;

#else
out vec4 FragColor;
#endif

void main()
{
    vec4 col = texture(material.matTexture, TexCoord) * material.colMultiplier;

    #ifdef TRANSPARENT

    float weight = clamp(
            pow(min(1.0, col.a * 10.0) + 0.01, 3.0) * 1e8
                * pow(1.0 - gl_FragCoord.z * 0.9, 3.0), 1e-2, 3e3
        );

    outAccum = vec4(col.rgb * col.a, col.a) * weight;

    outRevealage = col.a;
    #else
    if (col.a <= 0.99) {
        discard;
    }

    FragColor = col.rgba;
    #endif
}
