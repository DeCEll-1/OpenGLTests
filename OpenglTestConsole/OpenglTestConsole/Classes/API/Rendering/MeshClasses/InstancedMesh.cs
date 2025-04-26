using Newtonsoft.Json.Linq;
using OpenglTestConsole.Classes.API.Extensions;
using OpenglTestConsole.Classes.API.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
{
    public partial class InstancedMesh<T> where T : Mesh
    {
        public List<T> Meshes { get; set; } = new();
        public InstancedMesh() { }

        public void FinishAddingElemets()
        {
            Matrix4[] matrices = new Matrix4[Meshes.Count];
            for (int i = 0; i < Meshes.Count; i++)
            {
                matrices[i] = Meshes[i].Transform.GetModelMatrix();
            }
            // for GOD KNOWS WHY
            // matrix turns when i put them in the vertex shader, so i will turn them here (or chatgpt will ig idk m*th)
            Matrix4[] rotatedMatrixes = new Matrix4[matrices.Length];
            for (int i = 0; i < matrices.Length; i++)
            {
                Matrix4 matrix = matrices[i];
              
                rotatedMatrixes[i] = matrix.RotateMatrixForInstancedRendering();
            }
            SetMatrix4(rotatedMatrixes, 12, 1);
        }

        public void PrepareRender(Light light)
        {
            if (Meshes.Count == 0) return;

            var shader = Meshes[0].Shader;
            shader.Use();

            if (Meshes[0] is LightEffectedMesh lightMesh)
                lightMesh.SetStaticUniforms(light); // handles light uniforms

            GL.Enable(EnableCap.CullFace); // optional: control based on mesh settings
        }
        public void EndRender() => GL.Disable(EnableCap.CullFace);

        #region render
        public void Render(PrimitiveType type = PrimitiveType.Triangles)
        {
            T ourMesh = Meshes[0];
            ourMesh.Shader.SetMatrix4("projection", ourMesh.Camera.GetProjectionMatrix());
            ourMesh.Shader.SetMatrix4("view", ourMesh.Camera.GetViewMatrix());

            GL.BindVertexArray(ourMesh.VertexArrayObjectPointer);
            GL.DrawArraysInstanced(type, 0, ourMesh.size, Meshes.Count);
        }
        public void RenderWithIndices(PrimitiveType type = PrimitiveType.Triangles)
        {
            T ourMesh = Meshes[0];

            ourMesh.Shader.SetMatrix4("projection", ourMesh.Camera.GetProjectionMatrix());
            ourMesh.Shader.SetMatrix4("view", ourMesh.Camera.GetViewMatrix());

            GL.BindVertexArray(ourMesh.VertexArrayObjectPointer);
            GL.DrawElementsInstanced(type, ourMesh.indices.Length, DrawElementsType.UnsignedInt, 0, Meshes.Count); // render the mesh
        }
        #endregion
    }
}
