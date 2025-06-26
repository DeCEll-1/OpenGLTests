using OpenTK.Mathematics;
using RGL.API.Rendering.Materials;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Rendering.Shaders;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;
using RGL.Generated.Paths;
using System.Buffers;

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
            int[] viewport = new int[4];
            GL.GetInteger(GetIndexedPName.Viewport, 0, viewport);

            Vector2i size = new(viewport[2], viewport[3]);

            int bufferLength = 4 * size.X * size.Y; // RGBA * width * height

            // Allocate new byte array instead of renting from pool
            byte[] buffer = new byte[bufferLength];

            unsafe
            {
                fixed (byte* bufferPtr = buffer)
                {
                    GL.ReadPixels(
                        0,
                        0,
                        size.X,
                        size.Y,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        (nint)bufferPtr
                    );
                }
            }

            // Create texture from the byte array
            Texture tex = Texture.LoadFromBytes(buffer, size.X, size.Y);
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
