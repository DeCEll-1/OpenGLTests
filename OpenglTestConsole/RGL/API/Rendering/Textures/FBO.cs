using OpenTK.Mathematics;
using RGL.API.Misc;

namespace RGL.API.Rendering.Textures
{
    public struct TextureInfo
    {
        public PixelInternalFormat InternalFormat;
        public PixelFormat Format;
        public PixelType Type;
        public TextureInfo(PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
        {
            InternalFormat = internalFormat;
            Format = format;
            Type = type;
        }
    }

    public class FBO : IDisposable
    {
        private bool disposed = false;
        public string name = "";
        public int Handle { get; set; }
        //public int StencilRenderBuffer { get; private set; }
        public Texture ColorTexture { get; private set; }
        public Texture DepthStencilTexture { get; private set; }
        public FBO() { }
        public List<Texture> ColorTextures = new(); // supports MRT

        public void Init(Vector2i? size = null, string name = "", Texture? depthStencilTexture = null, TextureInfo[]? colorAttachments = null)
        {
            Logger.BeginTimingBlock();
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
                x = APISettings.Resolution.X; y = APISettings.Resolution.Y;
            }
            else
            {
                x = size.Value.X; y = size.Value.Y;
            }

            ColorTextures.Clear();


            if (colorAttachments != null && colorAttachments.Length > 0)
            {
                for (int i = 0; i < colorAttachments.Length; i++)
                {
                    var info = colorAttachments[i];
                    var tex = Texture.LoadFromSize(
                        x, y,
                        target: TextureTarget.Texture2D,
                        pixelInternalFormat: info.InternalFormat,
                        pixelFormat: info.Format,
                        pixelType: info.Type,
                        textureMinFilter: TextureMinFilter.Linear,
                        textureMagFilter: TextureMagFilter.Linear,
                        name: $"Color_{i}",
                        logCreation: false
                    );
                    ColorTextures.Add(tex);

                    //FramebufferAttachment attachmentType = FramebufferAttachment.ColorAttachment0;
                    //switch (i)
                    //{
                    //    case 0:
                    //        attachmentType = FramebufferAttachment.ColorAttachment0; break;
                    //    case 1:
                    //        attachmentType = FramebufferAttachment.ColorAttachment1; break;
                    //    default:
                    //        break;
                    //}

                    GL.FramebufferTexture2D(
                        FramebufferTarget.Framebuffer,
                        FramebufferAttachment.ColorAttachment0 + i,
                        TextureTarget.Texture2D,
                        tex.Handle,
                        0
                    );
                }
            }
            else
            {
                ColorTexture = Texture.LoadFromSize(
                    x, y,
                    target: TextureTarget.Texture2D,
                    pixelInternalFormat: PixelInternalFormat.Rgba,
                    pixelFormat: PixelFormat.Rgba,
                    pixelType: PixelType.UnsignedByte,
                    textureMinFilter: TextureMinFilter.Linear,
                    textureMagFilter: TextureMagFilter.Linear,
                    name: "Color",
                    logCreation: false
                );

                ColorTextures.Add(ColorTexture);

                GL.FramebufferTexture2D( // attatch the texture
                    FramebufferTarget.Framebuffer, // the frame buffer will write to this texture
                    FramebufferAttachment.ColorAttachment0, // attatchment type
                    TextureTarget.Texture2D, // texture type
                    ColorTexture.Handle,
                    0 // mipmap level
                );
            }

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
                    pixelType: PixelType.UnsignedInt248,
                    // wrap mode s
                    // wrap mode t
                    textureMinFilter: TextureMinFilter.Linear,
                    textureMagFilter: TextureMagFilter.Linear,
                    name: "Depth & Stencil",
                    logCreation: false
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
                Logger.Log(
                   $"An error occured while creating frame buffer for {LogColors.BrightWhite(Handle)} :\n" +
                   $"{GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString()}",
                   LogLevel.Error
                );
            }
            else
            {
                string colorAttachmentLog = "";

                if (ColorTextures.Count > 1)
                {
                    for (int i = 0; i < ColorTextures.Count; i++)
                    {
                        var tex = ColorTextures[i];
                        colorAttachmentLog +=
                            $"  Color Attachment {i}: {LogColors.BC("Texture")} {LogColors.BW(tex.Handle)} " +
                            $"{LogColors.BW(tex.width)}x{LogColors.BW(tex.height)}\n";
                    }
                }
                else if (ColorTexture != null)
                {
                    colorAttachmentLog =
                        $"  Color {LogColors.BC("Texture")}: {LogColors.BW(ColorTexture.Handle)} " +
                        $"{LogColors.BW(ColorTexture.width)}x{LogColors.BW(ColorTexture.height)}\n";
                }

                Logger.Log(
                    $"Loaded {LogColors.BC("FBO")} {LogColors.BW(Handle)}" +
                    $"{(string.IsNullOrWhiteSpace(name) ? "" : $", named {LogColors.BW(name)}")}:\n" +
                    colorAttachmentLog +
                    $"  Depth & Stencil {LogColors.BrightCyan("Texture")}: " +
                    $"{LogColors.BW(DepthStencilTexture.Handle)} " +
                    $"{LogColors.BW(DepthStencilTexture.width)}x{LogColors.BW(DepthStencilTexture.height)}\n" +
                    $"  In {LogColors.BG(Logger.EndTimingBlockFormatted())}",
                    LogLevel.Detail
                );
            }
            // execute victory dance
            this.name = name;
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

        #region disposal
        ~FBO()
        {
            if (disposed == false)
                Logger.Log(
                    $"GPU Resource leak for FBO! Did you forget to call Dispose()?",
                    LogLevel.Error
                );
        }

        protected virtual void Dispose(bool disposing, bool log = true)
        {
            if (!disposed)
            {
                if (log)
                {
                    //if (ColorTexture != null)
                    //{
                    ColorTexture.logDisposal = log;
                    ColorTexture.Dispose();
                    //}

                    //if (DepthStencilTexture != null)
                    //{
                    DepthStencilTexture.logDisposal = log;
                    DepthStencilTexture.Dispose();
                    //}

                    foreach (var tex in ColorTextures)
                    {
                        tex.logDisposal = log;
                        tex.Dispose();
                    }
                    ColorTextures.Clear();




                    Logger.Log(
            $"{LogColors.BrightYellow("Disposed")} {LogColors.BC("FBO")} {LogColors.BW(Handle)}{(name != null ? $", named {LogColors.BW(name)}" : "")}",
                    LogLevel.Detail
                    );
                }

                GL.DeleteFramebuffer(Handle);
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
