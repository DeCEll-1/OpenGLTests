using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Mesh
{
    public partial class InstancedMesh<T> where T : Mesh
    {

        public void SetMatrix4(Matrix4[] matrices, int loc, int offset = 1)
        {
            int vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, matrices.Length * Marshal.SizeOf<Matrix4>(), matrices, BufferUsageHint.StaticDraw);

            foreach (T mesh in Meshes)
            {
                GL.BindVertexArray(mesh.VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

                int matrixSize = Marshal.SizeOf<Matrix4>();

                // Set up each column (vec4) of the mat4 as a separate attribute
                for (int i = 0; i < 4; i++)
                {
                    int attribLocation = loc + i;
                    GL.EnableVertexAttribArray(attribLocation);
                    GL.VertexAttribPointer(attribLocation, 4, VertexAttribPointerType.Float, false, matrixSize, i * Vector4.SizeInBytes);
                    GL.VertexAttribDivisor(attribLocation, offset);
                }
            }
        }
    }
}
