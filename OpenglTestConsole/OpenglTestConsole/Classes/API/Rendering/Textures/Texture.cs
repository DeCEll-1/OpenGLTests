using OpenglTestConsole.Classes.API.Misc;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Reflection.Metadata;
using static OpenglTestConsole.Generated.Paths.ResourcePaths.Shaders;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
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
            Vector2i size = new(width, height);
            var image = Image.LoadPixelData<Rgba32>(GetBytes(), size.X, size.Y);
            image.Mutate(s => s.Flip(FlipMode.Vertical));
            if (filePath.Contains(".jpg") || filePath.Contains(".jpeg"))
                image.SaveAsJpeg(filePath);
            else
                image.SaveAsPng(filePath);
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
                if (log)
                    Logger.Log(
                    $"{LogColors.BY("Disposed")} {LogColors.BC("Texture")} {LogColors.BW(Handle)}{(name != "" ? $", named {LogColors.BW(name)}" : "")}",
                    LogLevel.Detail
                );

                GL.DeleteTexture(Handle);
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
