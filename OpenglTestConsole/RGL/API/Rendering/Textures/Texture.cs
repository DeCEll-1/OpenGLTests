using ImageMagick;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using RGL.API.Misc;
using System.Buffers;

namespace RGL.API.Rendering.Textures
{
    public partial class Texture : IDisposable
    {
        public int Handle { get; set; }
        public bool initalised = false;
        public int width, height;
        public bool disposed { get; private set; } = false;

        #region opengl functions
        public void Paramater(TextureParameterName name, int param)
        {
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                param
            );
        }
        private void Check()
        {
            if (initalised)
                return;
            Logger.Log($"Texture {Handle} used without initalisation", LogLevel.Error);
        }

        public void Bind()
        {
            Check();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Activate(TextureUnit unit)
        {
            Check();
            GL.ActiveTexture(unit);
        }
        #endregion

        public byte[] GetBytes()
        {
            byte[] output = new byte[
                4 *
                width *
                height
            ];

            unsafe
            {
                fixed (byte* outputPtr = output)
                {
                    Bind();
                    GL.GetTexImage(
                        TextureTarget.Texture2D,
                        0,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        (nint)outputPtr
                    );
                }
            }

            return output;
        }

        public void SaveToFile(string filePath)
        {
            MagickFormat format = MagickFormat.Rgb;
            if (this.PixelFormat == PixelFormat.Rgba)
            {
                format = MagickFormat.Rgba;
            }

            // Create MagickReadSettings for raw pixel data (assuming RGBA)
            var settings = new MagickReadSettings
            {
                Width = (uint?)width,
                Height = (uint?)height,
                Format = format,
            };
            using var ms = new MemoryStream(GetBytes());
            using var image = new MagickImage(ms, settings);


            // Flip vertically
            image.Flip();

            // Save as JPEG or PNG depending on file extension
            if (filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                image.Format = MagickFormat.Jpeg;
                image.Write(filePath);
            }
            else
            {
                image.Format = MagickFormat.Png;
                image.Write(filePath);
            }


            Logger.Log(
                $"Saved Texture with {LogColors.BrightWhite(Handle)} as {LogColors.BrightWhite(filePath)}",
                LogLevel.Detail
            );
        }

        #region disposal
        ~Texture()
        {
            if (disposed == false)
                Logger.Log(
                    $"GPU Resource leak for texture! Did you forget to call Dispose()?",
                    LogLevel.Error
                );
        }

        protected virtual void Dispose(bool disposing, bool log = true)
        {
            if (!disposed)
            {

                GL.DeleteTexture(Handle);
                if (log)
                    Logger.Log(
                    $"{LogColors.BY("Disposed")} {LogColors.BC("Texture")} {LogColors.BW(Handle)}{(name != "" ? $", named {LogColors.BW(name)}" : "")}",
                    LogLevel.Detail
                );
                Handle = 0;
                disposed = true;
            }
        }
        public bool logDisposal = true;

        public void Dispose()
        {
            Dispose(true, logDisposal);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
