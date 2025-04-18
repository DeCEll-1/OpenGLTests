using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenglTestConsole.Classes.API.JSON.MCSDFJSON;
using static System.Net.Mime.MediaTypeNames;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    public class Text : Mesh
    {
        private string _text = "Hello World!";
        public string TextString
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                UpdateText();
            }
        }

        private FontJson Font { get; set; }
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector2> texCoords = new List<Vector2>();
        public Vector4 ForeColor = new Vector4(1f, 1f, 1f, 1f);
        public Vector4 BackColor = new Vector4(0f, 0f, 0f, 1f);
        private Texture Texture;
        private float scale = 1f;

        [SetsRequiredMembers]
        public Text(Camera camera, string shader = "MCSDF") : base(camera, shader: shader)
        {

        }
        private void Init()
        {

        }
        private void UpdateText()
        {
            float penX = 0f;
            vertices.Clear();
            texCoords.Clear();
            foreach (char character in _text)
            {
                Glyph glyph = Font.glyphs.FirstOrDefault(g => g.unicode == character)!;

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
            this.size = vertices.Count;
        }

        public override void Render(PrimitiveType type = PrimitiveType.Triangles)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusConstantAlpha);

            this.Shader.Use();
            // our texture for text rendering
            this.Shader.SetTexture("msdf", this.Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            this.Shader.SetVector4("bgColor", this.BackColor);
            this.Shader.SetVector4("fgColor", this.ForeColor);
            this.Shader.SetFloat("pxRange", 1f); // precision or smthin idk
            this.Render(type);

            GL.Disable(EnableCap.Blend);
        }

    }
}
