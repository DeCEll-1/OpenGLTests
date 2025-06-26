using ImGuiNET;
using RGL.API.Rendering;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;
using RGL.Classes.API;
using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Materials;
using RGL.Classes.API.Rendering.Shaders;
using RGL.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGL.API.Rendering.Shaders;

namespace RGL.Classes.Implementations.RenderScripts.TestRSs
{
    public class PostProcessingTest : RenderScript
    {
        public Mesh Mesh { get; set; }
        public override void Init()
        {

            //PostProcessingMaterial PPMaterial = new PostProcessingMaterial(Resources.Shaders[ResourcePaths.Materials.PPInversion.Name]);
            //PostProcessingMaterial PPMaterial = new PPFogMaterial();
            PostProcessingMaterial PPMaterial = new PPGammaCorrection();



            PostProcess Effect = new PostProcess(PPMaterial);
            Scene.PostProcesses.Add(Effect);

        }
        public override void Advance()
        {

        }
    }
}
