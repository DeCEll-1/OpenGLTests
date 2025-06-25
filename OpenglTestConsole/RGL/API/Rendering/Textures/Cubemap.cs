﻿using RGL.API.Misc;

namespace RGL.API.Rendering.Textures
{
    public class Cubemap : IDisposable
    {
        // for prettier texture inits
        string[] faceNames =
        [
            "TextureCubeMapPositiveX",
            "TextureCubeMapNegativeX",
            "TextureCubeMapPositiveY",
            "TextureCubeMapNegativeY",
            "TextureCubeMapPositiveZ",
            "TextureCubeMapNegativeZ"
        ];
        public int Handle { get; set; }
        public bool initalised = false;
        private bool _disposed = false;
        public bool disposed
        {
            get
            { // İF all of the textures are disposed *and* this cubemap is disposed, then return disposed
                return textures.All(tex => tex.disposed) && _disposed == true;
            }
            private set => _disposed = value;
        }

        #region sides
        public Texture Right { get; init; }
        public Texture Left { get; init; }
        public Texture Top { get; init; }
        public Texture Bottom { get; init; }
        public Texture Back { get; init; }
        public Texture Front { get; init; }
        public Texture[] textures { get => [Right, Left, Top, Bottom, Front, Back]; }
        #endregion

        public Cubemap(Texture[] inTextures)
        {
            #pragma warning disable format
            Right   =   inTextures[0];      Left    =     inTextures[1];
            Top     =   inTextures[2];      Bottom  =     inTextures[3];
            Back    =   inTextures[4];      Front   =     inTextures[5];      
            #pragma warning restore format
        }

        public void Init()
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);


            if (textures.Any(tex => tex.initalised == true))
            { // check if any of the textures are initalized, we need them to not be initalized
                string initalisedTextures = "";
                foreach (Texture tex in textures)
                {
                    if (tex.initalised)
                        initalisedTextures += $"{LogColors.BrightWhite(tex.Handle)}, ";
                }

                Logger.Log($"Cubemap {LogColors.BrightWhite(Handle)} recieved initalised Texture(s): {initalisedTextures}", LogLevel.Error);
            }

            for (int i = 0; i < textures.Length; i++)
            {
                Texture tex = textures[i]; // set the cube map type of the texture
                tex.Target = TextureTarget.TextureCubeMapPositiveX + i;
                tex.flipped = false;
                // init it while we are binded to the cubemap, which will attatch the texture to the cubemap
                tex.Init(name: faceNames[i]);
            }

            /* 
                https://learnopengl.com/code_viewer_gh.php?code=src/4.advanced_opengl/6.2.cubemaps_environment_mapping/cubemaps_environment_mapping.cpp
                glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
                glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE); // x clamping
                glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE); // y clamping
                glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE); // z clamping
             */

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            Logger.Log(
                $"Loaded {LogColors.BrightCyan("Cubemap")} {LogColors.BrightWhite(Handle)}",
                LogLevel.Detail
            );

            initalised = true;
        }

        private void Check()
        {
            if (initalised)
                return;
            Logger.Log($"Cubemap {Handle} used without initalisation", LogLevel.Error);
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

        #region disposal
        ~Cubemap()
        {
            if (disposed == false)
                Logger.Log(
                    $"GPU Resource leak for cubemap! Did you forget to call Dispose()?",
                    LogLevel.Error
                );
        }

        protected virtual void Dispose(bool disposing, bool log = true)
        {
            if (!disposed)
            {
                if (log)
                    Logger.Log(
                    $"{LogColors.BrightYellow("Disposed")} Cubemap {LogColors.BrightWhite(Handle)}",
                    LogLevel.Detail
                );
                foreach (var texture in textures)
                    texture.Dispose();
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
