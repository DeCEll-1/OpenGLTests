using ICSharpCode.Decompiler.CSharp.Syntax;
using ImageMagick;
using RGL.API.Misc;
using System.Buffers;

namespace RGL.API.Rendering.Textures
{
    public partial class Texture
    {
        public TextureTarget Target { get; set; }
        public PixelInternalFormat PixelInternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public TextureWrapMode TextureSWrapMode { get; set; }
        public TextureWrapMode TextureTWrapMode { get; set; }
        public TextureMinFilter TextureMinFilter { get; set; }
        public TextureMagFilter TextureMagFilter { get; set; }
        public bool logCreation = true;
        private byte[]? bytes { get; set; }
        public bool flipped = true;
        public string name { get; private set; } = "";
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
        )
        => LoadFromTextureBytes(
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
            using var ms = new MemoryStream(bytes);
            using var image = new MagickImage(ms);
            using var pixelDataArray = image.GetPixels();

            PixelMapping mapping = pixelFormat == PixelFormat.Rgb ? PixelMapping.RGB : PixelMapping.RGBA;
            byte[] pixelArray = pixelDataArray.ToByteArray(0, 0, image.Width, image.Height, mapping)!;

            return LoadFromBytes(
                bytes: pixelArray,
                width: (int)image.Width,
                height: (int)image.Height,
                target: target,
                pixelInternalFormat: pixelInternalFormat,
                pixelFormat: pixelFormat,
                type: type,
                textureSWrapMode: textureSWrapMode,
                textureTWrapMode: textureTWrapMode,
                textureMinFilter: textureMinFilter,
                textureMagFilter: textureMagFilter
            );
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


        public void Init(string? name = "", bool isCubemap = false, int cubemapHandle = -1)
        {
            Logger.BeginTimingBlock();
            var paramTarget = this.Target;
            if (isCubemap)
            {
                this.Handle = cubemapHandle;
                paramTarget = TextureTarget.TextureCubeMap;
            }
            else
                this.Handle = GL.GenTexture();


            if (this.Handle == 0)
                Logger.Log(
                    $"Failed to generate texture for LoadFromBytes",
                LogLevel.Error
                );

            if (flipped)
            {
                try
                {
                    var imageLoadFormat = PixelFormat == PixelFormat.Rgb ? MagickFormat.Rgb : MagickFormat.Rgba;
                    var settings = new MagickReadSettings
                    {
                        Width = (uint)width,
                        Height = (uint)height,
                        Format = imageLoadFormat
                    };

                    using var ms = new MemoryStream(bytes!);
                    using var image = new MagickImage(ms, settings);
                    image.Flip();

                    using var pixelDataArray = image.GetPixels();
                    var mapping = PixelFormat == PixelFormat.Rgb ? PixelMapping.RGB : PixelMapping.RGBA;

                    bytes = pixelDataArray.ToByteArray(0, 0, image.Width, image.Height, mapping)!;
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error flipping image: {ex.ToString()}", LogLevel.Error);
                }
            }



            GL.BindTexture(paramTarget, this.Handle);

            GL.TexParameter(
                paramTarget,
                TextureParameterName.TextureWrapS,
                (int)TextureSWrapMode
            );

            GL.TexParameter(
                paramTarget,
                TextureParameterName.TextureWrapT,
                (int)TextureTWrapMode
            );

            GL.TexParameter(paramTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter);
            GL.TexParameter(paramTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter);

            GL.TexParameter(paramTarget, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(paramTarget, TextureParameterName.TextureMaxLevel, 0);

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

            this.bytes = null;

            this.name = name;
            if (logCreation)
                Logger.Log(
                    $"Loaded {LogColors.BC("Texture")} {LogColors.BrightWhite(this.Handle)}{(name != "" ? $", named {LogColors.BW(name)}" : "")} in {LogColors.BG(Logger.EndTimingBlockFormatted())}",
                    LogLevel.Detail
                );
        }

        public static Texture LoadFromSize( // shouldnt need an init
            int width,
            int height,
            TextureTarget target = TextureTarget.Texture2D,
            PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba,
            PixelFormat pixelFormat = PixelFormat.Rgba,
            PixelType pixelType = PixelType.UnsignedByte,
            TextureWrapMode textureSWrapMode = TextureWrapMode.Repeat,
            TextureWrapMode textureTWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter textureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter textureMagFilter = TextureMagFilter.Linear,
            string? name = "",
            bool logCreation = true
        )
        {
            Logger.BeginTimingBlock();
            Texture texture = new Texture();
            #region texture info
            texture.Target = target;
            texture.PixelInternalFormat = pixelInternalFormat;
            texture.PixelFormat = pixelFormat;
            texture.PixelType = pixelType;
            texture.TextureSWrapMode = textureSWrapMode;
            texture.TextureTWrapMode = textureTWrapMode;
            texture.TextureMinFilter = textureMinFilter;
            texture.TextureMagFilter = textureMagFilter;
            #endregion
            texture.Handle = GL.GenTexture();
            texture.name = name;
            texture.width = width;
            texture.height = height;


            GL.BindTexture(target, texture.Handle);

            GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)textureSWrapMode);

            if (target == TextureTarget.Texture2D)
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)textureTWrapMode);

            GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)textureMagFilter);

            GL.TexParameter(target, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(target, TextureParameterName.TextureMaxLevel, 0);

            // Texture creation
            if (target == TextureTarget.Texture1D)
            {
                GL.TexImage1D(
                    target,
                    0, // mipmap level
                    pixelInternalFormat,
                    width,
                    0, // border
                    pixelFormat,
                    pixelType,
                    nint.Zero // initialization pixels
                );
            }
            else if (target == TextureTarget.Texture2D)
            {
                GL.TexImage2D(
                    target,
                    0, // mipmap level
                    pixelInternalFormat,
                    width,
                    height,
                    0, // border
                    pixelFormat,
                    pixelType,
                    nint.Zero // initialization pixels
                );
            }

            texture.initalised = true;

            if (logCreation)
                Logger.Log( // one must sacrifice readability in the pursuit of nice colors
                    $"Loaded " +
                    $"{LogColors.BY("empty")} " +
                    $"{LogColors.BC("Texture")} " +
                    $"{LogColors.BW(texture.Handle)}" +
                    $"{(name != "" ? $", named {LogColors.BW(name)}" : "")}: " +
                    $"{LogColors.BW(width)}x{LogColors.BW(height)} in " +
                    $"{LogColors.BG(Logger.EndTimingBlockFormatted())}",
                LogLevel.Detail
            );
            return texture;
        }
    }
}
