using OpenTK.Mathematics;
using RGL.API.Rendering.Textures;
using RGL.API.SceneFolder;

namespace RGL.API.Misc
{
    public class RenderMisc
    {
        public static FBO GetScreenFBO(Scene Scene)
        {
            return Scene.PostProcesses.Count == 0
                ? Scene.MainFBO
                : Scene.pingPong.WriteTo;
        }
        public static Texture GetSceneTexture(Scene Scene)
        {
            FBO sourceFBO = GetScreenFBO(Scene);
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

    }
}
