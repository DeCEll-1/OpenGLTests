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
using OpenglTestConsole.classes.api.misc;

namespace OpenglTestConsole.classes.api.rendering
{
    public class Texture
    {
        public int Handle { get; set; }
        public bool initalised = false;
        public string path;
        public int width, height;

        public Texture(string path)
        {
            this.path = path;
        }

        public byte[] Init()
        {
            Handle = GL.GenTexture();
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image;
            try
            {
                image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occured while loading {LogColors.BrightWhite(path)} for {LogColors.BrightWhite(Handle)}:\n{ex.ToString()}", LogLevel.Error);
                Logger.Log($"Using default texture...", LogLevel.Warning);
                image = ImageResult.FromStream(File.OpenRead("Textures/PlaceHolder.png"), ColorComponents.RedGreenBlueAlpha);
            }

            GL.BindTexture(TextureTarget.Texture2D, Handle);

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
            width = image.Width;
            height = image.Height;

            initalised = true;
            Logger.Log($"Loaded texture {LogColors.BrightWhite(Handle)}: {LogColors.BrightWhite(path)}", LogLevel.Detail);
            return image.Data;
        }
        private void Check()
        {
            if (initalised)
                return;
            Logger.Log($"Texture {Handle} used without initalisation, initalising..", LogLevel.Warning);
            Init();
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

        public void GenerateMipmaps(byte[] data)
        {
            // i, really, dont know how to use this with fragment shader, so i wont use it
            // not like im rendering humongusly anyways...
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            nint pointer = handle.AddrOfPinnedObject();

            Bind();
            GL.TexStorage2D(TextureTarget2d.Texture2D, 4, SizedInternalFormat.Rgba8, width, height);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pointer);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

            handle.Free();
        }

    }
}
