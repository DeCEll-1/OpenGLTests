using System.Numerics;
using OpenglTestConsole.Classes.API.misc;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders.Compute
{
    public class ComputeShader : IDisposable
    { // https://learnopengl.com/Guest-Articles/2022/Compute-Shaders/Introduction
        public bool initalised = false;
        public string computeShaderPath;
        public int Handle;
        public ComputeShaderUnitManager UnitManager;
        public ShaderUniformManager UniformManager;
        private bool disposed = false;
        public Vector3 groupSize { get; private set; }

        public ComputeShader(string computeShaderPath)
        {
            this.computeShaderPath = computeShaderPath;
            UnitManager = new ComputeShaderUnitManager(Handle);
            UniformManager = new ShaderUniformManager(Handle);
        }

        public void Init()
        {
            Handle = GL.CreateProgram();

            int computeShaderPointer = HandleComputeShader(computeShaderPath);

            GL.AttachShader(Handle, computeShaderPointer);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int shaderLinkSuccess);

            if (shaderLinkSuccess == 0)
            {
                string errorLog = GL.GetProgramInfoLog(Handle);
                Logger.Log(
                    $"An error occured while loading compute shader for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}",
                    LogLevel.Error
                );
            }

            // get the group size
            int[] size = new int[3];
            GL.GetProgram(Handle, (GetProgramParameterName)All.ComputeWorkGroupSize, size);
            groupSize = new Vector3(size[0], size[1], size[2]);

            GL.DetachShader(Handle, computeShaderPointer);
            GL.DeleteShader(computeShaderPointer);

            initalised = true;
        }

        public int HandleComputeShader(string path)
        {
            string computeSource;

            try
            {
                computeSource = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Logger.Log(
                    $"An error occured while reading compute shader {LogColors.BrightWhite(path)}!\nError log:\n{e}",
                    LogLevel.Error
                );
                return -1;
            }

            int computeShaderPointer = GL.CreateShader(ShaderType.ComputeShader);

            GL.ShaderSource(computeShaderPointer, computeSource);

            GL.CompileShader(computeShaderPointer);

            GL.GetShader(
                computeShaderPointer,
                ShaderParameter.CompileStatus,
                out int shaderCompileSuccess
            );

            if (shaderCompileSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(computeShaderPointer);
                Logger.Log(
                    $"An error occured while loading compute shader {LogColors.BrightWhite(path)}!\nError log:\n{errorLog}",
                    LogLevel.Error
                );
            }

            return computeShaderPointer;
        }

        public void DispatchForSize(int x, int y, int z)
        {
            Dispatch((int)(x / groupSize.X), (int)(y / groupSize.Y), (int)(z / groupSize.Z));
        }

        public void Dispatch(int x, int y, int z)
        {
            Use();
            if (initalised == false)
            {
                Logger.Log(
                    $"Shader with {LogColors.BrightWhite(Handle)} used without initalisation, initalising..",
                    LogLevel.Warning
                );
                Init();
            }
            GL.DispatchCompute(x, y, z);
        }

        #region unimportants
        ~ComputeShader()
        {
            if (disposed == false)
                Logger.Log($"GPU Resource leak! Did you forget to call Dispose()?", LogLevel.Error);
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
                    $"{LogColors.BrightYellow("Disposed")} compute shader {LogColors.BrightWhite(Handle)}",
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
