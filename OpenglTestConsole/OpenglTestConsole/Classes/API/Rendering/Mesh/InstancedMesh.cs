using OpenglTestConsole.Classes.api.rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Mesh
{
    public partial class InstancedMesh<T> where T : Mesh
    {
        public List<T> Meshes { get; set; } = new();

        public void FinishAddingElemets()
        {
            Matrix4[] matrices = new Matrix4[Meshes.Count];
            for (int i = 0; i < Meshes.Count; i++)
            {
                matrices[i] = Meshes[i].Transform.GetModelMatrix();
            }
            SetMatrix4(matrices, 12, 1);
        }
        public void Render(PrimitiveType type = PrimitiveType.Triangles)
        {
            Meshes[0].Shader.SetMatrix4("projection", Meshes[0].Camera.GetProjectionMatrix());
            Meshes[0].Shader.SetMatrix4("view", Meshes[0].Camera.GetViewMatrix());

            foreach (Mesh mesh in Meshes)
            {
                GL.BindVertexArray(mesh.VertexArrayObjectPointer);
                GL.DrawArraysInstanced(type, 0, mesh.size, Meshes.Count);
            }
        }
        public void RenderWithIndices(PrimitiveType type = PrimitiveType.Triangles)
        {
            Meshes[0].Shader.SetMatrix4("projection", Meshes[0].Camera.GetProjectionMatrix());
            Meshes[0].Shader.SetMatrix4("view", Meshes[0].Camera.GetViewMatrix());

            foreach (Mesh mesh in Meshes)
            {
                GL.BindVertexArray(mesh.VertexArrayObjectPointer);
                GL.DrawElementsInstanced(type, mesh.indices.Length, DrawElementsType.UnsignedInt, 0, Meshes.Count); // render the mesh
            }
        }
    }
}
