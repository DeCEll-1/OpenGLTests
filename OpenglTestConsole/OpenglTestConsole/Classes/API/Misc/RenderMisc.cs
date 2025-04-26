using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Misc
{
    public class RenderMisc
    {
        public static Vector2[] DefaultTextureCoordinates
        {
            get
            {
                return [
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f)
                ];
            }
        }
    
        public static void EnableCullFace()=> GL.Enable(EnableCap.CullFace);
        public static void DisableCullFace() => GL.Disable(EnableCap.CullFace);


    }
}
