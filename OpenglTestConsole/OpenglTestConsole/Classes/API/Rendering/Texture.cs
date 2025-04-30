using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenglTestConsole.Classes.API.misc;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class Texture : IDisposable
    {
        public int Handle { get; set; }
        public bool initalised = false;
        public int width, height;
        private bool disposed = false;

        public Texture()
        {
        }

        public static Texture LoadFromFile(string path)
        {
            Texture texture = new();
            texture.Handle = GL.GenTexture();

            if (texture.Handle == 0)
                Logger.Log($"Failed to generate texture for {LogColors.BrightWhite(path)}", LogLevel.Error);

            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image;
            try
            {
                image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occured while loading {LogColors.BrightWhite(path)} for {LogColors.BrightWhite(texture.Handle)}:\n{ex.ToString()}", LogLevel.Error);
                // this aint working properly fix it sometime ig
                Logger.Log($"Using default texture...", LogLevel.Warning);
                image = ImageResult.FromStream(File.OpenRead("Resources/Textures/PlaceHolder.png"), ColorComponents.RedGreenBlueAlpha);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                image.Data
            );
            texture.width = image.Width;
            texture.height = image.Height;

            texture.initalised = true;
            Logger.Log($"Loaded texture {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite(path)}", LogLevel.Detail);
            return texture;
        }
        public static Texture LoadFromSize(int width, int height)
        {
            Texture texture = new Texture();
            texture.Handle = GL.GenTexture();
            texture.width = width;
            texture.height = height;
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                width,
                height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                IntPtr.Zero
            );
            texture.initalised = true;
            Logger.Log($"Loaded empty texture {LogColors.BrightWhite(texture.Handle)}: {LogColors.BrightWhite(width)}x{LogColors.BrightWhite(height)}", LogLevel.Detail);
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

        ~Texture()
        {
            if (disposed == false)
            {
                Logger.Log($"GPU Resource leak for texture! Did you forget to call Dispose()?", LogLevel.Error);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Logger.Log($"{LogColors.BrightYellow("Disposed")} texture {LogColors.BrightWhite(Handle)}", LogLevel.Detail);

                GL.DeleteTexture(Handle);
                Handle = 0;
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
