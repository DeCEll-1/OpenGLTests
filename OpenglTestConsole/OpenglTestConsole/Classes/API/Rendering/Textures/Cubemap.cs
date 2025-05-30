using OpenglTestConsole.Classes.API.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Textures
{
    public class Cubemap
    {
        public int Handle { get; init; }
        public bool initalised = false;
        private bool _disposed = false;
        public bool disposed
        {
            get
            { // İF all of the textures are disposed *and* this cubemap is disposed, then return disposed
                return textures.All(tex => tex.disposed) && this._disposed == true;
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
        public Texture[] textures { get => [Right, Left, Top, Bottom, Back, Front]; }
        #endregion

        public Cubemap(Texture[] inTextures)
        {
            #pragma warning disable format
            Right   =   inTextures[0];      Left    =     inTextures[1];
            Top     =   inTextures[2];      Bottom  =     inTextures[3];
            Front   =   inTextures[4];      Back    =     inTextures[5];      
            #pragma warning restore format

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);


            if (this.textures.Any(tex => tex.initalised == true))
            { // check if any of the textures are initalized, we need them to not be initalized
                string initalisedTextures = "";
                foreach (Texture tex in textures)
                {
                    if (tex.initalised)
                    {
                        initalisedTextures += $" {LogColors.BrightWhite(tex.Handle)}, ";
                    }
                }

                Logger.Log($"Cubemap {LogColors.BrightWhite(Handle)} recieved initalised Texture(s): {initalisedTextures}", LogLevel.Error);
            }

            for (int i = 0; i < textures.Length; i++)
            {
                Texture tex = textures[i]; // set the cube map type of the texture
                tex.Target = TextureTarget.TextureCubeMapPositiveX + i;
                tex.flipped = false;
                tex.Init(); // init it while we are binded to the cubemap, which will attatch the texture to the cubemap
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
                $"Initalized Cubemap {LogColors.BrightWhite(Handle)}",
                LogLevel.Detail
            );

            this.initalised = true;
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


    }
}
