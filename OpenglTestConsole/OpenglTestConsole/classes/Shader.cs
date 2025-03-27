using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.classes
{
    public class Shader : IDisposable
    {
        #region Main Shader Functions
        public int Handle;
        private bool disposed = false;
        public Shader(string vertexPath, string fragmentPath)
        {
            int vertShaderPointer = HandleVertexShader(vertexPath);

            int fragShaderPointer = HandleFragmentShader(fragmentPath);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertShaderPointer);
            GL.AttachShader(Handle, fragShaderPointer);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int shaderLinkSuccess);

            if (shaderLinkSuccess == 0)
            {
                string errorLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(errorLog);
            }

            GL.DetachShader(Handle, vertShaderPointer);
            GL.DetachShader(Handle, fragShaderPointer);
            GL.DeleteShader(vertShaderPointer);
            GL.DeleteShader(fragShaderPointer);
        }

        private int HandleFragmentShader(string path)
        {
            string fragSource = File.ReadAllText(path);

            int fragShaderPointer = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragShaderPointer, fragSource);

            GL.CompileShader(fragShaderPointer);

            GL.GetShader(fragShaderPointer, ShaderParameter.CompileStatus, out int fragShaderSuccess);

            if (fragShaderSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(fragShaderPointer);
                Console.WriteLine(errorLog);
            }

            Console.WriteLine("Loaded Fragment Shader For " + Handle + " : " + path);

            return fragShaderPointer;
        }
        private int HandleVertexShader(string path)
        {
            string vertSource = File.ReadAllText(path);

            int vertShaderPointer = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertShaderPointer, vertSource);

            GL.CompileShader(vertShaderPointer);

            GL.GetShader(vertShaderPointer, ShaderParameter.CompileStatus, out int vertShaderSuccess);

            if (vertShaderSuccess == 0)
            {
                string errorLog = GL.GetShaderInfoLog(vertShaderPointer);
                Console.WriteLine(errorLog);
            }

            Console.WriteLine("Loaded Vertex Shader: " + Handle + " : " + path);

            return vertShaderPointer;
        }
        ~Shader()
        {
            if (disposed == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }
        public void Use()
        {
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

        public void SetTexture(string name, Texture tex)
        {
            int loc = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(loc, tex.Handle);
        }



        #endregion

    }
}
