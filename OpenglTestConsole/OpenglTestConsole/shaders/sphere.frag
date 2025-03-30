#version 330 core

in vec3 FragPos; // moved frag pos
in vec3 Normal; // normals from the cpu
in vec2 TexCoord; // tex coordinates from the cpu

uniform sampler2D tex; // tex
uniform vec3 lightPos; // light pos
uniform vec3 lightColor; // light color
uniform vec3 viewPos; // camera

out vec4 FragColor;

void main()
{
    float ambientStrenght = .1f;
    vec3 ambient = ambientStrenght * lightColor;

    //We calculate the light direction, and make sure the normal is normalized.
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos); //Note: The light is pointing from the light to the fragment

    //The diffuse part of the phong model.
    //This is the part of the light that gives the most, it is the color of the object where it is hit by light.
    float diff = max(dot(norm, lightDir), 0.0); //We make sure the value is non negative with the max function.
    vec3 diffuse = diff * lightColor;

    //The specular light is the light that shines from the object, like light hitting metal.
    //The calculations are explained much more detailed in the web version of the tutorials.
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    vec3 specular = specularStrength * spec * lightColor;

    // Sample the texture
    vec4 texColor = texture(tex, TexCoord);

    vec3 res = (ambient + diffuse + specular) * texColor.rgb;

    // Apply lighting to the texture color
    FragColor = vec4(texColor.rgb, 1.0);
}
