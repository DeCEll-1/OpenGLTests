using OpenglTestConsole.Classes.API.Misc;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders
{
    public partial class Shader : IDisposable
    {
        #region Main Shader Functions
        public bool initalised = false;
        public string vertexPath;
        public string fragmentPath;
        public int Handle;
        private bool disposed = false;
        public ShaderUniformManager UniformManager;

        public Shader(string vertexPath, string fragmentPath)
        { this.vertexPath = vertexPath; this.fragmentPath = fragmentPath; }

        public void Init()
        {
            Handle = GL.CreateProgram();

            this.UniformManager = new ShaderUniformManager(Handle);

            int vertShaderPointer = HandleShader(vertexPath, ShaderType.VertexShader);

            int fragShaderPointer = HandleShader(fragmentPath, ShaderType.FragmentShader);

            GL.AttachShader(Handle, vertShaderPointer);
            GL.AttachShader(Handle, fragShaderPointer);

            // init the geometry shader, if we have one
            InitGeometry();

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int shaderLinkSuccess);

            if (shaderLinkSuccess == 0)
            {
                string errorLog = GL.GetProgramInfoLog(Handle);
                Logger.Log(
                    $"An error occured while loading shaders for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}",
                    LogLevel.Error
                );
            }

            GL.DetachShader(Handle, vertShaderPointer);
            GL.DetachShader(Handle, fragShaderPointer);
            GL.DeleteShader(vertShaderPointer);
            GL.DeleteShader(fragShaderPointer);

            initalised = true;
        }

        private int HandleShader(string path, ShaderType type)
        {
            string source;

            try
            {
                source = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"An error occured while loading {LogColors.BrightWhite(path)} for {LogColors.BrightWhite(Handle)} for {LogColors.BrightWhite(type.ToString())}:\n{ex.ToString()}",
                    LogLevel.Error
                );
                Logger.Log($"Exiting", LogLevel.Error);
                throw;
            }

            int shaderPointer = GL.CreateShader(type);

            GL.ShaderSource(shaderPointer, source);

            GL.CompileShader(shaderPointer);

            GL.GetShader(
                shaderPointer,
                ShaderParameter.CompileStatus,
                out int shaderSuccess
            );

            if (shaderSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(shaderPointer);
                Logger.Log(
                    $"An error occured while loading shaders for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}",
                    LogLevel.Error
                );
            }

            Logger.Log(
                $"Loaded {LogColors.BrightWhite(type)} shader for {LogColors.BrightWhite(Handle)} : {LogColors.BrightWhite(path)}",
                LogLevel.Detail
            );

            return shaderPointer;
        }

        ~Shader()
        {
            if (disposed == false)
            {
                Logger.Log(
                    $"GPU Resource leak for shader! Did you forget to call Dispose()?",
                    LogLevel.Error
                );
            }
        }

        public void Use()
        {
            if (initalised == false)
            {
                Logger.Log(
                    $"Shader with {LogColors.BrightWhite(Handle)} used without initalisation, initalising..",
                    LogLevel.Warning
                );
                Init();
            }
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                GL.DeleteProgram(Handle);
                Logger.Log(
                    $"{LogColors.BrightYellow("Disposed")} shader {LogColors.BrightWhite(Handle)}",
                    LogLevel.Detail
                );
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
