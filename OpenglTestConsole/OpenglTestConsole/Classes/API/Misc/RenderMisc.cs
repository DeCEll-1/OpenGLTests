using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Misc
{
    public class RenderMisc
    {
        public static Vector2[] DefaultTextureCoordinates
        {
            get
            {
                return
                [
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f),
                ];
            }
        }

        public static Texture GetScreenTexture()
        {
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

            return tex;
        }
    }
}
