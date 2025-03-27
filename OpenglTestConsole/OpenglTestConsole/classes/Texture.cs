using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenglTestConsole.classes
{
    public class Texture
    {
        public int Handle { get; set; }

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            Use();
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

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
            GenerateMipmaps();
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void GenerateMipmaps()
        {
            Use();
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }


    }
}
