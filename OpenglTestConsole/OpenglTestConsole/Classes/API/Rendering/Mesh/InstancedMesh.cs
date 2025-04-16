using Newtonsoft.Json.Linq;
using OpenglTestConsole.Classes.API.Rendering;
using OpenglTestConsole.Classes.Implementations.Rendering;
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
        public InstancedMesh() { }

        public void FinishAddingElemets()
        {
            Matrix4[] matrices = new Matrix4[Meshes.Count];
            for (int i = 0; i < Meshes.Count; i++)
            {
                //Meshes[i].Transform.Position = new Vector3(5f, 5f, 5f);
                matrices[i] = Meshes[i].Transform.GetModelMatrix();
            }
            //matrices = new Matrix4[1] { Matrix4.CreateTranslation(0f, 0f, 0f) }; // Example translation
            // for GOD KNOWS WHY
            // matrix turns when i put them in the vertex shader, so i will turn them here (or chatgpt will ig idk m*th)
            Matrix4[] rotatedMatrixes = new Matrix4[matrices.Length];
            for (int i = 0; i < matrices.Length; i++)
            {
                Matrix4 matrix = matrices[i];
                // Extract translation from last row
                Vector3 translation = new Vector3(matrix.M41, matrix.M42, matrix.M43);

                // Create a fixed matrix with translation in the last column
                Matrix4 fixedMatrix = new Matrix4(
                matrix.M11, matrix.M12, matrix.M13, translation.X,
                matrix.M21, matrix.M22, matrix.M23, translation.Y,
                matrix.M31, matrix.M32, matrix.M33, translation.Z,
                0, 0, 0, 1
                );
                rotatedMatrixes[i] = fixedMatrix;
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
            T ourMesh = Meshes[0];

            ourMesh.Shader.SetMatrix4("projection", ourMesh.Camera.GetProjectionMatrix());
            ourMesh.Shader.SetMatrix4("view", ourMesh.Camera.GetViewMatrix());
            ourMesh.Shader.SetMatrix4("model", ourMesh.Transform.GetModelMatrix());

            GL.BindVertexArray(ourMesh.VertexArrayObjectPointer);
            GL.DrawElementsInstanced(type, ourMesh.indices.Length, DrawElementsType.UnsignedInt, 0, Meshes.Count); // render the mesh
        }
        #endregion
    }
}
