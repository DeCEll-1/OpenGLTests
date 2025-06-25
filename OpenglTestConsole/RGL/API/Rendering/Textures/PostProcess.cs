using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;

namespace RGL.API.Rendering.Textures
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

        public void Apply(int FBOToWriteTo, FBO FBOToReadFrom)
        {
            FBO.BindToFBO(FBOToWriteTo);


            Effect.FBOToReadFrom = FBOToReadFrom;
            ScreenMesh.Render();

            FBO.SetToDefaultFBO();
        }



    }
}
