using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.Implementations.Classes;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class FBO
    {
        public int Handle { get; set; }
        public FBO()
        {

        }
        public void Init()
        {
            // you just HAD to do shit with pointers
            unsafe
            {
                uint temp = 0; // create the fbo
                GL.CreateFramebuffers(1, &temp);
                Handle = (int)temp;
            }

            // bind the fbo so we are working on it
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
            int x = Settings.Resolution.X;
            int y = Settings.Resolution.Y;

            // create texture thats the same size as the window (not required)
            // TODO: change this when we get settings for window size or w/e
            Texture colorTexture = Texture.LoadFromSize(x, y);

            GL.FramebufferTexture2D( // attatch the texture
                FramebufferTarget.Framebuffer, // the frame buffer will write to this texture
                FramebufferAttachment.ColorAttachment0, // attatchment type
                TextureTarget.ProxyTexture2D, // texture type
                colorTexture.Handle,
                0 // mipmap level
             );

            // create the depth and stencil texture
            Texture depthAndStencilTexture = Texture.LoadFromSize(
                x,
                y,
                target: TextureTarget.Texture2D,
                pixelInternalFormat: PixelInternalFormat.Depth24Stencil8,
                pixelFormat: PixelFormat.DepthStencil,
                type: PixelType.UnsignedInt248
             );

            // bind the depth and stencil texture
            GL.FramebufferTexture2D( // same as the above texture 2d
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthStencilAttachment,
                TextureTarget.Texture2D,
                depthAndStencilTexture.Handle,
                0
             );

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Misc.Logger.Log(
                   $"An error occured while creating frame buffer for {Misc.LogColors.BrightWhite(Handle)} :\n" +
                   $"{GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString()}",
                   Misc.LogLevel.Error
                );
            }
            else
            {
                Misc.Logger.Log(
                    $"Loaded FBO {Misc.LogColors.BrightWhite(Handle)}:\n" +
                    $"Color Texture: {Misc.LogColors.BrightWhite(colorTexture.Handle)}" +
                    $"Depth & Stencil: {Misc.LogColors.BrightWhite(depthAndStencilTexture.Handle)}",
                   Misc.LogLevel.Detail
                );
            }
            // execute victory dance
        }


    }
}
