using OpenglTestConsole.Classes.API.Misc;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Reflection.Metadata;
using static OpenglTestConsole.Generated.Paths.ResourcePaths.Materials;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
{
    public class Texture : IDisposable
    {
        public int Handle { get; set; }
        public bool initalised = false;
        public int width, height;
        public bool disposed { get; private set; } = false;

        #region creation
        public TextureTarget Target { get; set; }
        public PixelInternalFormat PixelInternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public TextureWrapMode TextureSWrapMode { get; set; }
        public TextureWrapMode TextureTWrapMode { get; set; }
        public TextureMinFilter TextureMinFilter { get; set; }
        public TextureMagFilter TextureMagFilter { get; set; }
        private byte[] bytes { get; set; }
        public bool flipped = true;
        public Texture() { }

        public static Texture LoadFromFile(
            string path,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte,
            TextureWrapMode textureSWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode textureTWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter textureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter textureMagFilter = TextureMagFilter.Linear
        ) => LoadFromTextureBytes(
                 File.ReadAllBytes(path),
                 target: target,
                 pixelInternalFormat: pixelInternalFormat,
                 pixelFormat: pixelFormat,
                 type: type,
                 textureSWrapMode: textureSWrapMode,
                 textureTWrapMode: textureTWrapMode,
                 textureMinFilter: textureMinFilter,
                 textureMagFilter: textureMagFilter
            );


        public static Texture LoadFromTextureBytes(
            byte[] bytes,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte,
            TextureWrapMode textureSWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode textureTWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter textureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter textureMagFilter = TextureMagFilter.Linear
        )
        {

            Image<Rgba32> image;

            try
            {
                image = Image.Load<Rgba32>(bytes);
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"An error occured while loading {LogColors.BrightWhite("LoadFromTextureBytes")}:\n{ex.ToString()}",
                    LogLevel.Error
                );
                // this aint working properly fix it sometime ig
                Logger.Log($"Using default texture...", LogLevel.Warning);
                image = Image.Load<Rgba32>(File.ReadAllBytes("Resources/Textures/PlaceHolder.png"));
            }


            byte[] pixelDataArray = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(pixelDataArray);

            Texture texture = LoadFromBytes(
                bytes: pixelDataArray,
                width: image.Width,
                height: image.Height,
                target: target,
                pixelInternalFormat: pixelInternalFormat,
                pixelFormat: pixelFormat,
                type: type,
                textureSWrapMode: textureSWrapMode,
                textureTWrapMode: textureTWrapMode,
                textureMinFilter: textureMinFilter,
                textureMagFilter: textureMagFilter
            );
            image.Dispose();

            return texture;
        }

        public static Texture LoadFromBytes(
            byte[] bytes,
            int width,
            int height,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte,
            TextureWrapMode textureSWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode textureTWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter textureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter textureMagFilter = TextureMagFilter.Linear
        )
        {
            Texture texture = new();
            texture.bytes = bytes;
            texture.width = width;
            texture.height = height;
            texture.Target = target;
            texture.PixelInternalFormat = pixelInternalFormat;
            texture.PixelFormat = pixelFormat;
            texture.PixelType = type;
            texture.TextureSWrapMode = textureSWrapMode;
            texture.TextureTWrapMode = textureTWrapMode;
            texture.TextureMinFilter = textureMinFilter;
            texture.TextureMagFilter = textureMagFilter;

            return texture;
        }

        public void Init(string? name = null)
        {
            this.Handle = GL.GenTexture();

            if (flipped)
            {
                var image = Image.LoadPixelData<Rgba32>(
                    data: bytes,
                    width: width,
                    height: height
                );
                image.Mutate(s => s.Flip(FlipMode.Vertical));
                image.CopyPixelDataTo(bytes);
                image.Dispose();
            }

            if (this.Handle == 0)
                Logger.Log(
                    $"Failed to generate texture for LoadFromBytes",
                LogLevel.Error
                );

            GL.BindTexture(TextureTarget.Texture2D, this.Handle);

            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureSWrapMode
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureTWrapMode
            );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);


            GL.TexImage2D(
                this.Target,
                0,
                this.PixelInternalFormat,
                width,
                height,
                0,
                this.PixelFormat,
                this.PixelType,
                bytes
            );
            this.initalised = true;
            this.bytes = [];
            Logger.Log(
                $"Loaded {LogColors.BC("Texture")} {LogColors.BrightWhite(this.Handle)}{(name != null ? $", named {LogColors.BW(name)}" : "")}",
                LogLevel.Detail
            );
        }

        public static Texture LoadFromSize( // shouldnt need an init
            int width,
            int height,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte,
            TextureWrapMode textureSWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode textureTWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter textureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter textureMagFilter = TextureMagFilter.Linear
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
                (int)textureSWrapMode
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)textureTWrapMode
            );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);


            GL.TexImage2D(
                target,
                0, // mipmap level
                pixelInternalFormat,
                width,
                height,
                0, // border
                pixelFormat,
                type,
                nint.Zero // initilisation pixels
            );
            texture.initalised = true;
            Logger.Log(
                $"Loaded {LogColors.BrightYellow("empty")} {LogColors.BrightCyan("Texture")} {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite(width)}x{LogColors.BrightWhite(height)}",
                LogLevel.Detail
            );
            return texture;
        }
        #endregion

        #region opengl functions
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
                    $"{LogColors.BrightYellow("Disposed")} Texture {LogColors.BrightWhite(Handle)}",
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
