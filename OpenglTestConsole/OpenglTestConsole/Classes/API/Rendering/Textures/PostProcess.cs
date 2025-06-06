using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
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
        public PostProcessingMaterial Effect { get => (PostProcessingMaterial)this.ScreenMesh.Material; private set => this.ScreenMesh.Material = value; }
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
