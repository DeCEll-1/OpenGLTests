using ImGuiNET;
using OpenglTestConsole.Classes.API.Rendering.Textures;
using OpenglTestConsole.Classes.API.SceneFolder;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Misc
{
    public class RenderMisc
    {

        public static Texture GetScreenTexture()
        {
            Scene Scene = Main.mainScene;
            FBO sourceFBO = Scene.PostProcesses.Count == 0
                ? Scene.MainFBO
                : Scene.pingPong.ReadFrom;
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
                        (IntPtr)outputPtr
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
