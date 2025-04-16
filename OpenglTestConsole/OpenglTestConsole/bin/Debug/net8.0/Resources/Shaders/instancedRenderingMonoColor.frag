#version 330 core

in vec3 FragPos; // moved frag pos
in vec3 Normal; // normals from the cpu
in vec4 Color; // tex coordinates from the cpu

uniform vec3 lightPos; // light pos
uniform vec4 lightColorIn; // light color
uniform vec4 ambientIn; // ambient color
uniform vec3 viewPos; // camera

out vec4 FragColor;

void main()
{
    vec3 ambient = ambientIn.rgb * ambientIn.a; // multiply the color by alpha because wwe dont want transparent textures
    vec3 lightColor = lightColorIn.rgb * lightColorIn.a;

    //We calculate the light direction, and make sure the normal is normalized.
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos); //Note: The light is pointing from the light to the fragment

    //The diffuse part of the phong model.
    //This is the part of the light that gives the most, it is the color of the object where it is hit by light.
    float diff = max(dot(norm, lightDir), 0.0); //We make sure the value is non negative with the max function.
    vec3 diffuse = diff * lightColor;

    // //The specular light is the light that shines from the object, like light hitting metal.
    // //The calculations are explained much more detailed in the web version of the tutorials.
    // float specularStrength = 0.5;
    // vec3 viewDir = normalize(viewPos - FragPos);
    // vec3 reflectDir = reflect(-lightDir, norm);
    // float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    // vec3 specular = specularStrength * spec * lightColor;

    // vec3 res = (ambient + diffuse + specular) * (color.rgb * color.a);
    vec3 res = (ambient + diffuse); //* (Color.rgb * Color.a);

    // Apply lighting to the texture color
    // FragColor = vec4(res.rgb, 1.0);
    FragColor = vec4(1.0);
}
