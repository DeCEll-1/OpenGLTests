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
            ScreenMesh = new Mesh(new ScreenGeometry(), effect);
        }
        public PostProcessingMaterial Effect { get => (PostProcessingMaterial)this.ScreenMesh.Material; private set => this.ScreenMesh.Material = value; }
        private Mesh ScreenMesh { get; set; }

        public void Apply(int FBOToWriteTo, FBO FBOToReadFrom)
        {
            FBO.BindToFBO(FBOToWriteTo);


            Effect.FBOToReadFrom = FBOToReadFrom;
            ScreenMesh.Render();

            FBO.SetToDefaultFBO();
        }



    }
}
