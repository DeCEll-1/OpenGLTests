using OpenglTestConsole.classes.api.misc;
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

namespace OpenglTestConsole.classes.api.rendering
{
    public class Shader : IDisposable
    {
        #region Main Shader Functions
        public bool initalised = false;
        public string vertexPath; public string fragmentPath;
        public int Handle;
        private bool disposed = false;
        public Shader(string vertexPath, string fragmentPath)
        {
            this.vertexPath = vertexPath;
            this.fragmentPath = fragmentPath;
        }

        public void Init()
        {
            Handle = GL.CreateProgram();

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
                fragSource = File.ReadAllText("Shaders/default.frag");
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
                vertSource = File.ReadAllText("Shaders/default.vert");
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
                Logger.Log($"GPU Resource leak for shader {LogColors.BrightWhite(Handle)}! Did you forget to call Dispose()?", LogLevel.Error);
                Logger.Log($"Disposing {LogColors.BrightWhite(Handle)}...", LogLevel.Warning);
                GL.DeleteProgram(Handle);
            }
        }
        public void Use()
        {
            if (initalised == false)
                Logger.Log($"Shader with {LogColors.BrightWhite(Handle)} used without initalisation, initalising..", LogLevel.Warning);
            GL.UseProgram(Handle);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                GL.DeleteProgram(Handle);
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Uniform Functions

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(loc, true, ref matrix);
        }

        public void SetVector4(string name, Vector4 vector)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform4(loc, vector);
        }

        public void SetVector3(string name, Vector3 vector)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(loc, vector);
        }

        public void SetVector2(string name, Vector2 vector)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform2(loc, vector);
        }

        public void SetFloat(string name, float value)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(loc, value);
        }

        public void SetInt(string name, int value)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(loc, value);
        }

        public void SetColor(string name, Color4 color)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform4(loc, color);
        }

        public void SetTexture(string name, Texture tex, OpenTK.Graphics.OpenGL4.TextureUnit unit)
        {
            tex.Activate(unit);
            tex.Bind();
            int loc = GL.GetUniformLocation(Handle, name);

            int unitint = (int)unit - (int)TextureUnit.Texture0;

            GL.Uniform1(loc, unitint);
        }



        #endregion

    }
}
