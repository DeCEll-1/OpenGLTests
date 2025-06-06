using OpenglTestConsole.Classes.API;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Materials;
using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenglTestConsole.Classes.API.Rendering.Shaders.Compute;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Generated.Paths;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts.TestRSs
{
    public class ComputeShaderTest : RenderScript
    {
        private Texture Texture;
        private ComputeShader Shader;
        private Mesh Mesh;

        public override void Init()
        {
            this.Texture = Texture.LoadFromSize(
                512,
                512,
                pixelInternalFormat: PixelInternalFormat.Rgba32f,
                type: PixelType.Float, 
                name: "ComputeShaderTestTexture"
            );
            this.Shader = Resources.CompShaders[ResourcePaths.ComputeShaders.MyComputeShader.Name];
            Shader.UnitManager.SetImageTexture(this.Texture.Handle, 0);

            Square geometry = new Square(new(1));
            TextureMaterial material = new TextureMaterial(Texture);

            this.Mesh = new Mesh(geometry, material, name: "Compute shader test");
            //this.Mesh.CapsToEnable.Add(EnableCap.CullFace);
            this.Mesh.Transform.Position.Y = 3;
            this.Mesh.Transform.UpdateMatrix();

            this.Scene.Add(this.Mesh);
        }

        public override void Advance()
        {
            Shader.UnitManager.ApplyTextures();
            Shader.DispatchForSize(512, 512, 1);
            //GL.MemoryBarrier(MemoryBarrierFlags.ShaderImageAccessBarrierBit);
        }
    }
}
