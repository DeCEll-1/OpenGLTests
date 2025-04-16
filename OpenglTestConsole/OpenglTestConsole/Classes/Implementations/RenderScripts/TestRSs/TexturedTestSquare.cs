using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.api.rendering;
using OpenglTestConsole.Classes.API.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.Paths;
using OpenglTestConsole.Classes.API.Rendering.Mesh;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    internal class TexturedTestSquare : RenderScript
    {
        public Mesh SquareTextured { get; set; }
        public Texture Texture { get; set; }
        public override void Init()
        {
            SquareTextured = new Mesh(Camera);
            SquareTextured.size = 4;
            SquareTextured.SetVector3(
                //Square.GetSquare(400, 300),
                new Vector3[]
                {
                    new Vector3(-0.5f, 0.5f, 0f),
                    new Vector3(0.5f, 0.5f, 0f),
                    new Vector3(-0.5f, -0.5f, 0f),
                    new Vector3(0.5f, -0.5f, 0f)
                },
                0
            );
            SquareTextured.SetVector2(
                RenderMisc.DefaultTextureCoordinates,
                1
            );
            SquareTextured.InitShader(ResourcePaths.Shaders.texture); 

            Texture = Resources.Textures[ResourcePaths.Textures.PlaceHolder_png];
        }

        public override void Render()
        {
            SquareTextured.Transform.Position = Vector3.UnitX * 3f;
            //SquareTextured.Transform.SetRotation(x: 90f);
            SquareTextured.Shader.Use();
            SquareTextured.Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            SquareTextured.Render();
        }
    }
}
