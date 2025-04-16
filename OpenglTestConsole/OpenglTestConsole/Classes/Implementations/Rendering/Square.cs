using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.api.rendering;
using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.Paths;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    public class Square : LightEffectedMesh
    {
        public override int size { get => 4; set => base.size = value; }
        public Vector3 LeftTop { get; set; }
        public Vector3 RightTop { get; set; }
        public Vector3 LeftBottom { get; set; }
        public Vector3 RightBottom { get; set; }
        public Texture Texture { get; set; } = null;
        public Vector4 Color;
        public bool UsesTexture { get { return this.Texture != null; } }
        [SetsRequiredMembers]
        public Square(Camera camera, float size, Texture texture = null, Vector4? col = null) : base(camera)
        {
            float width = size / 2f;
            float height = size / 2f;
            LeftTop = new(-width, height, 0.0f);
            RightTop = new(width, height, 0.0f);
            LeftBottom = new(-width, -height, 0.0f);
            RightBottom = new(width, -height, 0.0f);
            if (col == null) { this.Texture = texture; } else { this.Color = col.Value; }
            Init();
        }
        [SetsRequiredMembers]
        public Square(Camera camera, Vector3[] vectors, Texture texture = null, Vector4? color = null) : base(camera)
        {
            LeftTop = vectors[0];
            RightTop = vectors[1];
            LeftBottom = vectors[2];
            RightBottom = vectors[3];
            if (color == null) { this.Texture = texture; } else { this.Color = color.Value; }
            Init();
        }
        private void Init()
        {
            if (this.UsesTexture)
                Shader = Resources.Shaders[ResourcePaths.Shaders.objectTextured];
            else
                Shader = Resources.Shaders[ResourcePaths.Shaders.objectMonoColor];

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);


            // https://community.khronos.org/t/normal-for-square/61082
            // idk how normals work sob
            var a = RightTop - LeftTop;
            var b = LeftBottom - LeftTop;
            Vector3.Cross(a, b, out Vector3 normal);

            SetVector3([LeftTop, RightTop, LeftBottom, RightBottom], 0);
            SetVector3([normal, normal, normal, normal], 1);

            if (this.UsesTexture)
                SetVector2(RenderMisc.DefaultTextureCoordinates, 2);

            this.indices = [0, 1, 2, 2, 1, 3];
            SetIndices(this.indices);
        }

        public new void Render(OpenTK.Graphics.OpenGL4.PrimitiveType type = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles)
        {
            //Shader.Use();

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);

            //GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
            Render(indices, type);
            //GL.Disable(EnableCap.CullFace);
        }
        public override void PrepareRender(Light light)
        {
            this.Shader.Use();
            this.SetStaticUniforms(light);
        }
        public override void EndRender() { }

    }
}
