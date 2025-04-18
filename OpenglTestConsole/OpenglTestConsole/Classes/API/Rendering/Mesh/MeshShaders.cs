﻿using OpenTK.Graphics.OpenGL;
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

        private Dictionary<int, int> _vboCache = new(); // loc -> VBO

        public void SetVector2(Vector2[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (!_vboCache.TryGetValue(loc, out int vbo))
            {
                // Create new buffer and cache it
                vbo = GL.GenBuffer();
                _vboCache[loc] = vbo;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(loc, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                GL.EnableVertexAttribArray(loc);
            }
            else
            {
                // Update existing buffer
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vectors.Length * Vector2.SizeInBytes, vectors);
            }
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
