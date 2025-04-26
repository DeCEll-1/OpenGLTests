using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
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

        public void SetVector4(Vector4[] vectors, int loc, int offset = 1)
        {
            int VBOPointer = GL.GenBuffer();

            // generate vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector4.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer

            foreach (T mesh in Meshes)
            {
                GL.BindVertexArray(mesh.VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

                GL.EnableVertexAttribArray(loc); // enable loc 0

                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0); // bind the buffer to location 0

                GL.VertexAttribDivisor(loc, offset);
            }
        }


        public Y[] GetFieldValuesFromMeshes<Y>(string fieldOrPropertyName)
        { // is this dumb? probably, but i dont care
            return Meshes.Select(mesh =>
            {
                var type = mesh.GetType();

                // Try to get a field first
                var field = type.GetField(fieldOrPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null && field.FieldType == typeof(Y))
                    return (Y)field.GetValue(mesh)!;

                // Try to get a property if field not found
                var prop = type.GetProperty(fieldOrPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.PropertyType == typeof(Y))
                    return (Y)prop.GetValue(mesh)!;

                throw new Exception($"Field or property '{fieldOrPropertyName}' not found or not of type {typeof(Y).Name} in mesh.");
            }).ToArray()!;
        }

    }
}
