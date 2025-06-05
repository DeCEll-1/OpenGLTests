using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
{
    public class FBO
    {
        public int Handle { get; set; }
        public Texture ColorTexture { get; private set; }
        public Texture DepthStencilTexture { get; private set; }
        public int StencilRenderBuffer { get; private set; }
        public FBO() { }
        public void Init(Vector2i? size = null, string name = "", Texture? colorTexture = null, Texture? depthStencilTexture = null)
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
            int x, y; x = y = 0;
            if (size == null)
            {
                x = Settings.Resolution.X; y = Settings.Resolution.Y;
            }
            else
            {
                x = size.Value.X; y = size.Value.Y;
            }

            if (colorTexture == null)
            {
                // create texture thats the same size as the window (not required)
                ColorTexture = Texture.LoadFromSize(
                    x, y,
                    target: TextureTarget.Texture2D,
                    pixelInternalFormat: PixelInternalFormat.Rgba,
                    pixelFormat: PixelFormat.Rgba,
                    type: PixelType.UnsignedByte,
                    // wrap mode s
                    // wrap mode t
                    textureMinFilter: TextureMinFilter.Linear,
                    textureMagFilter: TextureMagFilter.Linear
                );
            }
            else
            {
                ColorTexture = colorTexture;
            }

            GL.FramebufferTexture2D( // attatch the texture
                FramebufferTarget.Framebuffer, // the frame buffer will write to this texture
                FramebufferAttachment.ColorAttachment0, // attatchment type
                TextureTarget.Texture2D, // texture type
                ColorTexture.Handle,
                0 // mipmap level
            );

            /* however i need to sample them now
            // create render buffer, as we wont be reading the depth and stencil as texture so it can stay as render buffer
            // which is faster 
            StencilRenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, StencilRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.StencilIndex8, x, y);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0); // unbind

            // attatch the render buffer
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, StencilRenderBuffer);
            */

            if (depthStencilTexture == null)
            {
                DepthStencilTexture = Texture.LoadFromSize(
                    x, y,
                    target: TextureTarget.Texture2D,
                    pixelInternalFormat: PixelInternalFormat.Depth24Stencil8,
                    pixelFormat: PixelFormat.DepthStencil,
                    type: PixelType.UnsignedInt248,
                    // wrap mode s
                    // wrap mode t
                    textureMinFilter: TextureMinFilter.Linear,
                    textureMagFilter: TextureMagFilter.Linear
                );
            }
            else
            {
                DepthStencilTexture = depthStencilTexture;
            }


            GL.FramebufferTexture2D( // attatch the texture
                FramebufferTarget.Framebuffer, // the frame buffer will write to this texture
                FramebufferAttachment.DepthStencilAttachment, // attatchment type
                TextureTarget.Texture2D, // texture type
                DepthStencilTexture.Handle,
                0 // mipmap level
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
                Logger.Log(
                    $"Loaded {LogColors.BC("FBO")} {LogColors.BW(Handle)}{(name != null ? $", named {LogColors.BW(name)}" : "")}:\n" +
                    $"Color {LogColors.BC("Texture")}: {LogColors.BW(ColorTexture.Handle)}\n" +
                    $"Depth & Stencil {LogColors.BrightCyan("Texture")}: {LogColors.BW(DepthStencilTexture.Handle)}",
                    LogLevel.Detail
                );
            }
            // execute victory dance
            Unbind();
        }

        public void Bind()
        => GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);

        public void Unbind()
        // as this isnt like shaders (you cant forget a shader binded dude comeon)
        // we want to add an unbind here so its easier to unbind this as staying binded to it can and WİLL cause problems if 
        // forgotten
        => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        // also nice to have
        public static void BindToFBO(int handle) => GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
        public static void SetToDefaultFBO() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

    }
}
