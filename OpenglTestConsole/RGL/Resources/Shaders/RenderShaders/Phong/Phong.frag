#version 330 core

in vec3 FragPos0; // moved frag pos
in vec3 Normal0; // normals from the cpu
flat in vec3 flatNormal0; // normals from the cpu
in vec2 TexCoord0;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};
uniform Material material;

//The light contains all the values from the light source, how the ambient diffuse and specular values are from the light source.
//This is technically what we were using in the last episode as we were only applying the phong model directly to the light.
struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;

uniform vec3 viewPos; // camera

out vec4 FragColor;

in vec3 EdgeDistance0;

void main()
{
    //ambient
    vec3 ambient = light.ambient * material.ambient; //Remember to use the material here.

    //diffuse
    vec3 norm = normalize(Normal0);
    vec3 lightDir = normalize(light.position - FragPos0);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse); //Remember to use the material here.

    //specular
    vec3 viewDir = normalize(viewPos - FragPos0);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * material.specular); //Remember to use the material here.

    //Now the result sum has changed a bit, since we now set the objects color in each element, we now dont have to
    //multiply the light with the object here, instead we do it for each element seperatly. This allows much better control
    //over how each element is applied to different objects.
    vec3 result = ambient + diffuse + specular;

    float d = min(EdgeDistance0.x, min(EdgeDistance0.y, EdgeDistance0.z)) * 3.;

    // if (d < .05)
    //     result = vec3(1.);
    // else
    //     result = vec3(0.);

    FragColor = vec4(result, 1.);
}
