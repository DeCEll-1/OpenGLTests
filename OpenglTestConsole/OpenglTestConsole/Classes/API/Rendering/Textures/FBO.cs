using OpenglTestConsole.Classes.API.Misc;
using OpenglTestConsole.Classes.Implementations.Classes;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
{
    public class FBO
    {
        public int Handle { get; set; }
        public Texture ColorTexture { get; private set; }
        public int RenderBuffer { get; private set; }
        public FBO() { }
        public void Init(Vector2i? size = null, string name = "")
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


            // create texture thats the same size as the window (not required)
            ColorTexture = Texture.LoadFromSize(
                x, y,
                target: TextureTarget.Texture2D,
                pixelInternalFormat: PixelInternalFormat.Rgba,
                pixelFormat: PixelFormat.Rgba,
                type: PixelType.UnsignedByte,


                textureMinFilter: TextureMinFilter.Linear,
                textureMagFilter: TextureMagFilter.Linear
            );

            GL.FramebufferTexture2D( // attatch the texture
                FramebufferTarget.Framebuffer, // the frame buffer will write to this texture
                FramebufferAttachment.ColorAttachment0, // attatchment type
                TextureTarget.Texture2D, // texture type
                ColorTexture.Handle,
                0 // mipmap level
            );

            // create render buffer, as we wont be reading the depth and stencil as texture so it can stay as render buffer
            // which is faster 
            RenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, x, y);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0); // unbind

            // attatch the render buffer
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer);


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
                    $"Depth & Stencil {LogColors.BrightCyan("Render Buffer")}: {LogColors.BW(RenderBuffer)}",
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
