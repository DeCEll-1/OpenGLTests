using OpenglTestConsole.classes;
using OpenglTestConsole.classes.api.rendering;
using OpenglTestConsole.Classes.API.JSON;
using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;
using static System.Net.Mime.MediaTypeNames;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    internal class TextRendering : RenderScript
    {
        private FontJson Font { get; set; }
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> texCoords = new List<Vector2>();
        Mesh Mesh;
        Texture Texture;

        public override void Init()
        {
            Font = MCSDFJSON.GetFontJson("Resources/Fonts/ComicSans.json");
            Texture = Main.Textures["Resources/Fonts/ComicSans.png"];
            float scale = 1.0f;
            string text = "god daymn!!!";
            float penX = 0f;
            foreach (char character in text)
            {
                Glyph glyph = Font.glyphs.FirstOrDefault(g => g.unicode == character);

                if (glyph == null || glyph.planeBounds == null)
                {
                    penX += glyph?.advance ?? 0f; // Skip if glyph not found or planeBounds is null
                    continue; // Skip if glyph not found or planeBounds is null
                }

                Bounds planeBounds = glyph.planeBounds;
                Bounds atlasBounds = glyph.atlasBounds;
                float w = Font.atlas.width;
                float h = Font.atlas.height;

                // Vertex positions
                float x0 = (penX + planeBounds.left) * scale;
                float y0 = planeBounds.bottom * scale;
                float x1 = (penX + planeBounds.right) * scale;
                float y1 = planeBounds.top * scale;

                // Texture coords
                float u0 = atlasBounds.left / w;
                float v0 = atlasBounds.bottom / h;
                float u1 = atlasBounds.right / w;
                float v1 = atlasBounds.top / h;

                vertices.Add(new Vector3(x0, y0, 0));
                vertices.Add(new Vector3(x1, y0, 0));
                vertices.Add(new Vector3(x0, y1, 0));

                vertices.Add(new Vector3(x1, y0, 0));
                vertices.Add(new Vector3(x0, y1, 0));
                vertices.Add(new Vector3(x1, y1, 0));


                texCoords.Add(new Vector2(u0, v0));
                texCoords.Add(new Vector2(u1, v0));
                texCoords.Add(new Vector2(u0, v1));

                texCoords.Add(new Vector2(u1, v0));
                texCoords.Add(new Vector2(u0, v1));
                texCoords.Add(new Vector2(u1, v1));

                penX += glyph.advance;
            }

            Mesh = new Mesh(this.Camera, vertices.Count, "MCSDF");
            Mesh.SetVector3(vertices.ToArray(), 0);
            Mesh.SetVector2(texCoords.ToArray(), 1);
            Mesh.Transform.Position = new Vector3(0f, 0f, -3f);
        }

        public override void Render()
        {

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusConstantAlpha);

            Mesh.Shader.Use();
            Mesh.Shader.SetTexture("msdf", this.Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            Mesh.Shader.SetVector4("bgColor", new(0f));
            Mesh.Shader.SetVector4("fgColor", new(1f));
            Mesh.Shader.SetFloat("pxRange", 1f); // precision or smthin

            Mesh.Render(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles);

            GL.Disable(EnableCap.Blend);
        }
    }
}
