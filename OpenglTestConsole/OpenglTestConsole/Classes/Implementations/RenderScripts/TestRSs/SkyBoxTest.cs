using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class SkyBoxTest : RenderScript
    {
        public Mesh Mesh { get; set; }
        public override void Init()
        {
            Skybox geometry = new Skybox();

            Cubemap map = Resources.Cubemaps[ResourcePaths.Cubemaps.Sea.Name];

            SkyboxMaterial material = new SkyboxMaterial(map);


            this.Mesh = new Mesh(geometry, material);
            this.Mesh.type = PrimitiveType.Triangles;


            Scene.Add(Mesh);

        }
        public override void BeforeRender() => GL.DepthFunc(DepthFunction.Lequal);
        public override void AfterRender() => GL.DepthFunc(DepthFunction.Less);
        public override void Advance()
        {

        }
    }
}
