using OpenglTestConsole.Classes.API.misc;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Shaders
{
    public class Shader : IDisposable
    {
        #region Main Shader Functions
        public bool initalised = false;
        public string vertexPath; public string fragmentPath;
        public int Handle;
        private bool disposed = false;
        public ShaderUniformManager UniformManager;
        public Shader(string vertexPath, string fragmentPath)
        {
            this.vertexPath = vertexPath;
            this.fragmentPath = fragmentPath;
        }

        public void Init()
        {
            Handle = GL.CreateProgram();

            this.UniformManager = new ShaderUniformManager(Handle);

            int vertShaderPointer = HandleVertexShader(vertexPath);

            int fragShaderPointer = HandleFragmentShader(fragmentPath);

            GL.AttachShader(Handle, vertShaderPointer);
            GL.AttachShader(Handle, fragShaderPointer);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int shaderLinkSuccess);

            if (shaderLinkSuccess == 0)
            {
                string errorLog = GL.GetProgramInfoLog(Handle);
                Logger.Log($"An error occured while loading shaders for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}", LogLevel.Error);
            }

            GL.DetachShader(Handle, vertShaderPointer);
            GL.DetachShader(Handle, fragShaderPointer);
            GL.DeleteShader(vertShaderPointer);
            GL.DeleteShader(fragShaderPointer);

            initalised = true;
        }
        private int HandleFragmentShader(string path)
        {
            string fragSource;
            try
            {
                fragSource = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occured while loading {LogColors.BrightWhite(path)} for {LogColors.BrightWhite(Handle)}:\n{ex.ToString()}", LogLevel.Error);
                Logger.Log($"Using default fragment shader...", LogLevel.Warning);
                fragSource = File.ReadAllText("Resources/Shaders/default.frag");
            }

            int fragShaderPointer = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragShaderPointer, fragSource);

            GL.CompileShader(fragShaderPointer);

            GL.GetShader(fragShaderPointer, ShaderParameter.CompileStatus, out int fragShaderSuccess);

            if (fragShaderSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(fragShaderPointer);
                Logger.Log($"An error occured while loading shaders for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}", LogLevel.Error);
            }

            Logger.Log($"Loaded fragment shader for shader {LogColors.BrightWhite(Handle)} : {LogColors.BrightWhite(path)}", LogLevel.Detail);

            return fragShaderPointer;
        }
        private int HandleVertexShader(string path)
        {
            string vertSource;

            try
            {
                vertSource = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occured while loading {LogColors.BrightWhite(path)} for {LogColors.BrightWhite(Handle)}:\n{ex.ToString()}", LogLevel.Error);
                Logger.Log($"Using default vertex shader...", LogLevel.Warning);
                vertSource = File.ReadAllText("Resources/Shaders/default.vert");
            }

            int vertShaderPointer = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertShaderPointer, vertSource);

            GL.CompileShader(vertShaderPointer);

            GL.GetShader(vertShaderPointer, ShaderParameter.CompileStatus, out int vertShaderSuccess);

            if (vertShaderSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(vertShaderPointer);
                Logger.Log($"An error occured while loading shaders for {LogColors.BrightWhite(Handle)}!\nError log:\n{errorLog}", LogLevel.Error);
            }

            Logger.Log($"Loaded vertex shader for {LogColors.BrightWhite(Handle)} : {LogColors.BrightWhite(path)}", LogLevel.Detail);

            return vertShaderPointer;
        }
        ~Shader()
        {
            if (disposed == false)
            {
                Logger.Log($"GPU Resource leak for shader! Did you forget to call Dispose()?", LogLevel.Error);
            }
        }
        public void Use()
        {
            if (initalised == false)
            {
                Logger.Log($"Shader with {LogColors.BrightWhite(Handle)} used without initalisation, initalising..", LogLevel.Warning);
                Init();
            }
            GL.UseProgram(Handle);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                GL.DeleteProgram(Handle);
                Logger.Log($"{LogColors.BrightYellow("Disposed")} shader {LogColors.BrightWhite(Handle)}", LogLevel.Detail);
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
