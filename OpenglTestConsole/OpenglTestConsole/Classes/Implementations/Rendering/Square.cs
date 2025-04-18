using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.API.Rendering;
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
        public Square(Camera camera, Vector3[] vectors, Vector4 color, string shader) : base(camera)
        {
            LeftTop = vectors[0];
            RightTop = vectors[1];
            LeftBottom = vectors[2];
            RightBottom = vectors[3];
            this.Color = color;
            Init(shader);
        }
        [SetsRequiredMembers]
        public Square(Camera camera, Vector3[] vectors, Texture texture, string shader) : base(camera)
        {
            LeftTop = vectors[0];
            RightTop = vectors[1];
            LeftBottom = vectors[2];
            RightBottom = vectors[3];
            this.Texture = texture;
            Init(shader);
        }
        private void Init(string shader)
        {
            //if (this.UsesTexture)
            //    Shader = Resources.Shaders[ResourcePaths.Shaders.objectTextured];
            //else
            //    Shader = Resources.Shaders[ResourcePaths.Shaders.objectMonoColor];
            Shader = Resources.Shaders[shader];

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);


            // https://community.khronos.org/t/normal-for-square/61082
            // idk how normals work sob
            var a = RightTop - LeftTop;
            var b = LeftBottom - LeftTop;
            Vector3.Cross(a, b, out Vector3 normal);

            BufferManager.SetVector3([LeftTop, RightTop, LeftBottom, RightBottom], 0);
            BufferManager.SetVector3([normal, normal, normal, normal], 1);

            if (this.UsesTexture)
                BufferManager.SetVector2(RenderMisc.DefaultTextureCoordinates, 2);

            this.indices = [0, 1, 2, 2, 1, 3];
            BufferManager.SetIndices(this.indices);
        }

        public override void Render(OpenTK.Graphics.OpenGL4.PrimitiveType type = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles)
        {
            //Shader.Use();

            if (this.UsesTexture)
                Shader.SetTexture("tex", Texture, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            else
                Shader.SetVector4("color", this.Color);

            //GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
            base.Render(indices, type);
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
