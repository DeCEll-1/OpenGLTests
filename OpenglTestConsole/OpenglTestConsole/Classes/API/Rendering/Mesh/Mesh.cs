using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.Classes.api.misc;
using OpenglTestConsole.Classes.Paths;
using OpenglTestConsole.Classes.api.rendering;

namespace OpenglTestConsole.Classes.API.Rendering.Mesh
{
    public partial class Mesh
    {
        #region Init
        /// <summary>
        /// The corner amount of the mesh
        /// </summary>
        public uint[] indices;
        public virtual int size { get; set; }
        public required Camera Camera { get; set; } = new Camera(800, 600);
        public required Shader Shader { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public int VertexArrayObjectPointer { get; private set; }
        [SetsRequiredMembers]
        public Mesh(Camera camera, int size = 3, string shader = ResourcePaths.Shaders.defaultShader)
        {
            this.size = size;
            InitShader(shader);

            VertexArrayObjectPointer = GL.GenVertexArray();
            Camera = camera;
        }
        #endregion


        #region Render  
        private void Prep()
        {
            if (Shader.initalised == false)
            {
                Logger.Log($"Mesh {LogColors.BrightWhite(VertexArrayObjectPointer)} used without shader initalisation, initalising..", LogLevel.Warning);
                Shader.Init();
            }
        }
        public void Render(PrimitiveType type = PrimitiveType.TriangleStrip)
        {
            Prep();
            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Shader.SetMatrix4("model", Transform.GetModelMatrix());


            GL.BindVertexArray(VertexArrayObjectPointer);
            GL.DrawArrays(type, 0, size);
        }
        public void Render(uint[] indices, PrimitiveType type = PrimitiveType.Triangles)
        { // deadass render that shit cuh 
            // on it boss ima render that shit cuh
            Prep();

            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Shader.SetMatrix4("model", Transform.GetModelMatrix());

            GL.BindVertexArray(VertexArrayObjectPointer);

            GL.DrawElements(type, indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        #endregion


    }
}
