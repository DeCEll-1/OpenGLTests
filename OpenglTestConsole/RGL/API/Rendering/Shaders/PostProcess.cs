using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials.PPMaterials;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;

namespace RGL.API.Rendering.Shaders
{
    public class PostProcess
    {
        public PostProcess(PostProcessingMaterial effect)
        {
            ScreenMesh = new Mesh(new ScreenGeometry(), effect, name: effect.Shader.name);
            ScreenMesh.CapsToDisable.Add(EnableCap.DepthTest);
            ScreenMesh.BeforeRender = delegate { GL.DepthMask(false); };
            ScreenMesh.AfterRender = delegate { GL.DepthMask(true); };

        }
        public PostProcessingMaterial Effect { get => (PostProcessingMaterial)ScreenMesh.Material; private set => ScreenMesh.Material = value; }
        public Mesh ScreenMesh { get; private set; }

        public void Apply(int FBOToWriteTo, FBO FBOToReadFrom, Scene scene)
        {
            FBO.BindToFBO(FBOToWriteTo);


            Effect.FBOToReadFrom = FBOToReadFrom;
            ScreenMesh.Render(scene);

            FBO.SetToDefaultFBO();
        }



    }
}
