namespace RGL.Classes.API.Rendering.Shaders
{
    public partial class Shader
    { // geometry part of the shader
        public string geometrySource { get; set; } = "NULL";
        public Shader(string vertexSource, string fragmentSource, string geometrySource, string name = "")
        { this.vertexSource = vertexSource; this.fragmentSource = fragmentSource; this.geometrySource = geometrySource; this.name = name ?? ""; }
        private void InitGeometry()
        {
            if (this.geometrySource == "NULL")
                return;

            int geomShaderPointer = HandleShader(geometrySource, ShaderType.GeometryShader);

            GL.AttachShader(Handle, geomShaderPointer);
        }
    }
}
