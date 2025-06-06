using OpenglTestConsole.Classes.API.Misc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
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
        private byte[] bytes { get; set; }
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

        public void Init(string? name = "")
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

            this.name = name;
            Logger.Log(
                $"Loaded {LogColors.BC("Texture")} {LogColors.BrightWhite(this.Handle)}{(name != "" ? $", named {LogColors.BW(name)}" : "")}",
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
            string? name = ""
        )
        {
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
                pixelType,
                nint.Zero // initilisation pixels
            );
            texture.initalised = true;
            
            Logger.Log( // one must sacrifice readability in the pursuit of nice colors
$"Loaded {LogColors.BY("empty")} {LogColors.BC("Texture")} {LogColors.BW(texture.Handle)}{(name != "" ? $", named {LogColors.BW(name)}": "")}: {LogColors.BW(width)}x{LogColors.BW(height)}",
                LogLevel.Detail
            );
            return texture;
        }
    }
}
