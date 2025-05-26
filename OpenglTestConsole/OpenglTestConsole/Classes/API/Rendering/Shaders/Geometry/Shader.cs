using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders
{
    public partial class Shader
    {
        public string geometryPath { get; set; } = "NULL";
        public Shader(string vertexPath, string fragmentPath, string geometryPath)
        { this.vertexPath = vertexPath; this.fragmentPath = fragmentPath; this.geometryPath = geometryPath; }
        private void InitGeometry()
        {
            if (this.geometryPath == "NULL")
                return;

            int geomShaderPointer = HandleShader(geometryPath, ShaderType.GeometryShader);

            GL.AttachShader(Handle, geomShaderPointer);
        }
    }
}
