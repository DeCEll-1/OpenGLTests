using OpenTK.Mathematics;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Generated.Paths;

namespace RGL.API.Misc
{
    public class RenderMisc
    {
        public static FBO GetSceneFBO(Scene scene)
        {
            return scene.MainFBO;
        }
        public static Texture GetSceneTexture(Scene scene)
        {
            FBO sourceFBO = GetSceneFBO(scene);
            int[] k = new int[4];

            GL.GetInteger(GetIndexedPName.Viewport, 0, k);

            Vector2i size = new(k[2], k[3]);


            byte[] output = new byte[
                4 *
                size.X *
                size.Y
            ];

            unsafe
            {
                fixed (byte* outputPtr = output)
                {
                    GL.ReadPixels(
                        0,
                        0,
                        size.X,
                        size.Y,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        (nint)outputPtr
                    );
                }
            }

            Texture tex = Texture.LoadFromBytes(output, k[2], k[3]);
            tex.flipped = false;

            FBO.SetToDefaultFBO();
            return tex;
        }

        private static PostProcess passthroughProcess = new PostProcess(
                    new PostProcessingMaterial(
                        Resources.Shaders[RGLResources.Shaders.PPWriteFBO.Name]
                    )
                );

        public static void RenderSceneToScreen(Scene scene)
        {
            passthroughProcess.Apply(FBOToWriteTo: 0, FBOToReadFrom: scene.MainFBO, scene);
        }

    }
}
