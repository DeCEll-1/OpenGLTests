using ImGuiNET;
using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
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
