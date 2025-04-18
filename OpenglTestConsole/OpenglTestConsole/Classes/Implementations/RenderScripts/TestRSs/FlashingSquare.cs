using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using OpenglTestConsole.Classes.Implementations.Rendering;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class FlashingSquare : RenderScript
    {
        public Mesh Square { get; set; }
        public override void Init()
        {
            Square = new Mesh(Camera);
            Square.size = 4;
            Square.BufferManager.SetVector3(
                //Square.GetSquare(400, 300),
                [
                    new Vector3(-0.5f, 0.5f, 0f),
                    new Vector3(0.5f, 0.5f, 0f),
                    new Vector3(-0.5f, -0.5f, 0f),
                    new Vector3(0.5f, -0.5f, 0f)
                ],
                0
            );
            Square.InitShader(ResourcePaths.ShaderNames.greenBlink);
            Square.Transform.Position = Vector3.UnitZ * 3f;
        }

        public override void Render()
        {
            Square.Shader.Use();
            Square.Shader.SetFloat("t", (float)Timer.Elapsed.TotalSeconds);
            Square.Render();
        }
    }
}
