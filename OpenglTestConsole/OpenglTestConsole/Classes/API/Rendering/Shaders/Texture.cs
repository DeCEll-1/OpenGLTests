using OpenglTestConsole.Classes.API.Misc;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders
{
    public class Texture : IDisposable
    {
        public int Handle { get; set; }
        public bool initalised = false;
        public int width,
            height;
        private bool disposed = false;

        public Texture() { }

        public static Texture LoadFromFile(
            string path,
            PixelInternalFormat format = PixelInternalFormat.Rgba,
            PixelType type = PixelType.UnsignedByte
        )
        {
            Image<Rgba32> image;

            try
            {
                image = Image.Load<Rgba32>(File.ReadAllBytes(path));
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"An error occured while loading {LogColors.BrightWhite(path)}:\n{ex.ToString()}",
                    LogLevel.Error
                );
                // this aint working properly fix it sometime ig
                Logger.Log($"Using default texture...", LogLevel.Warning);
                image = Image.Load<Rgba32>(File.ReadAllBytes("Resources/Textures/PlaceHolder.png"));
            }


            byte[] pixelDataArray = new byte[image.Width * image.Height * 4];
            image.Mutate(s => s.Flip(FlipMode.Vertical));
            image.CopyPixelDataTo(pixelDataArray);

            Texture texture = LoadFromBytes(bytes: pixelDataArray, width: image.Width, height: image.Height, format: format, type: type);

            Logger.Log(
                $"Loaded texture {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite(path)}",
                LogLevel.Detail
            );
            return texture;
        }

        public static Texture LoadFromBytes(
            byte[] bytes,
            int width,
            int height,
            PixelInternalFormat format = PixelInternalFormat.Rgba,
            PixelType type = PixelType.UnsignedByte
            )
        {
            Texture texture = new();
            texture.Handle = GL.GenTexture();

            if (texture.Handle == 0)
                Logger.Log(
                    $"Failed to generate texture for LoadFromBytes",
                    LogLevel.Error
                );

            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            texture.width = width;
            texture.height = height;

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Repeat
            );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                format,
                width,
                height,
                0,
                PixelFormat.Rgba,
                type,
                bytes
            );
            texture.width = width;
            texture.height = height;

            texture.initalised = true;
            Logger.Log(
                $"Loaded texture {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite("FromBytes")}",
                LogLevel.Detail
            );
            return texture;
        }

        public static Texture LoadFromSize(
            int width,
            int height,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte
        )
        {
            Texture texture = new Texture();
            texture.Handle = GL.GenTexture();
            texture.width = width;
            texture.height = height;
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(
                target,
                0,
                pixelInternalFormat,
                width,
                height,
                0,
                pixelFormat,
                type,
                nint.Zero
            );
            texture.initalised = true;
            Logger.Log(
                $"Loaded empty texture {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite(width)}x{LogColors.BrightWhite(height)}",
                LogLevel.Detail
            );
            return texture;
        }

        private void Check()
        {
            if (initalised)
                return;
            Logger.Log($"Texture {Handle} used without initalisation..", LogLevel.Error);
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

        public byte[] GetBytes()
        {
            byte[] output = new byte[
                4 *
                this.width *
                this.height
            ];

            unsafe
            {
                fixed (byte* outputPtr = output)
                {
                    this.Bind();
                    GL.GetTexImage(
                        TextureTarget.Texture2D,
                        0,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        (IntPtr)outputPtr
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
                    $"{LogColors.BrightYellow("Disposed")} texture {LogColors.BrightWhite(Handle)}",
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
    }
}
