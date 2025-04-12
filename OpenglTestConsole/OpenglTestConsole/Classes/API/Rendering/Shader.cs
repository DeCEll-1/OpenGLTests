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
                Logger.Log($"GPU Resource leak! Did you forget to call Dispose()?", LogLevel.Error);
            }
        }
        public void Use()
        {
            if (initalised == false)
            {
                Logger.Log($"Shader with {LogColors.BrightWhite(Handle)} used without initalisation, initalising..", LogLevel.Warning);
                this.Init();
            }
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
        // cache uniforms so we dont run *expensive* get uniform location a shit ton
        private Dictionary<string, int> uniformCache = new Dictionary<string, int>();
        public void SetMatrix4(string name, Matrix4 matrix)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.UniformMatrix4(loc, true, ref matrix);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetMatrix4(name, matrix);
            }
        }

        public void SetVector4(string name, Vector4 vector)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform4(loc, vector);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetVector4(name, vector);
            }
        }

        public void SetVector3(string name, Vector3 vector)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform3(loc, vector);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetVector3(name, vector);
            }
        }

        public void SetVector2(string name, Vector2 vector)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform2(loc, vector);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetVector2(name, vector);
            }
        }

        public void SetFloat(string name, float value)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform1(loc, value);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetFloat(name, value);
            }
        }

        public void SetInt(string name, int value)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform1(loc, value);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetInt(name, value);
            }
        }

        public void SetColor(string name, Color4 color)
        {
            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                GL.Uniform4(loc, color);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetColor(name, color);
            }
        }

        public void SetTexture(string name, Texture tex, OpenTK.Graphics.OpenGL4.TextureUnit unit)
        {
            tex.Activate(unit);
            tex.Bind();

            if (uniformCache.ContainsKey(name))
            {
                uniformCache.TryGetValue(name, out int loc);
                int unitint = (int)unit - (int)TextureUnit.Texture0;
                GL.Uniform1(loc, unitint);
            }
            else
            {
                uniformCache.Add(name, GL.GetUniformLocation(Handle, name));
                SetTexture(name, tex, unit);
            }
        }

        #endregion

    }
}
