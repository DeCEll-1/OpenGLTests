using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Mesh
{
    public partial class Mesh
    {

        #region Shader
        public void InitShader(string shader)
        {
            Shader = Resources.Shaders[shader];
        }
        public void SetVector2(Vector2[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer

            GL.VertexAttribPointer(loc, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetVector3(Vector3[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer

            GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetVector4(Vector4[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector4.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer


            GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetColor(Color4[] colors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * 4 * sizeof(float), colors, BufferUsageHint.StaticDraw); // put data in buffer


            GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetMatrix4(Matrix4[] matrices, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, matrices.Length * 16 * sizeof(float), matrices, BufferUsageHint.StaticDraw); // put data in buffer
            GL.VertexAttribPointer(loc, 16, VertexAttribPointerType.Float, false, 16 * sizeof(float), 0); // bind the buffer to location 0
            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetIndices(uint[] indices)
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }
        #endregion

    }
}
