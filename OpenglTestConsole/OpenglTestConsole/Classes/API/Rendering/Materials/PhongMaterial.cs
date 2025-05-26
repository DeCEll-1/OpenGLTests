using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenTK.Mathematics;
using OpenglTestConsole.Generated.Paths;

namespace OpenglTestConsole.Classes.API.Rendering.Materials
{
    public partial class PhongMaterial : Material
    {
        // color of the ambient lightning
        public Vector3 Ambient { get; set; }

        // color of the diffuse lightning
        public Vector3 Diffuse { get; set; }

        // color of the specular lightning
        public Vector3 Specular { get; set; }

        // size of the specular light
        public float Shininess { get; set; }

        public PhongMaterial()
        {
            this.Shader = Resources.Shaders[ResourcePaths.Materials.Phong.Name];
        }

        public PhongMaterial(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Shininess = shininess * 128f;
            this.Shader = Resources.Shaders[ResourcePaths.Materials.Phong.Name];
        }

        public override void Apply()
        {
            Shader.UniformManager.SetVector3("material.ambient", Ambient);
            Shader.UniformManager.SetVector3("material.diffuse", Diffuse);
            Shader.UniformManager.SetVector3("material.specular", Specular);
            Shader.UniformManager.SetFloat("material.shininess", Shininess);

            // update this to allow more lights
            Shader.UniformManager.SetVector3("light.position", Scene.Lights[0].Position);
            Shader.UniformManager.SetVector3("light.specular", Scene.Lights[0].Specular);
            Shader.UniformManager.SetVector3("light.diffuse", Scene.Lights[0].Diffuse);
            Shader.UniformManager.SetVector3("light.ambient", Scene.Lights[0].Ambient);

            Shader.UniformManager.SetVector3("viewPos", Scene.Camera.Position);
        }
    }
}
