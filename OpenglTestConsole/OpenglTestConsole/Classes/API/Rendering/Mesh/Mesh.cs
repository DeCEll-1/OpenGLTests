using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.Classes.API.misc;
using OpenglTestConsole.Classes.Paths;
using OpenglTestConsole.Classes.API.Rendering;

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
        public BufferManager BufferManager { get; }
        public int VertexArrayObjectPointer { get; private set; }
        [SetsRequiredMembers]
        public Mesh(Camera camera, int size = 3, string shader = ResourcePaths.ShaderNames.defaultShader)
        {
            this.size = size;
            InitShader(shader);

            VertexArrayObjectPointer = GL.GenVertexArray();
            BufferManager = new BufferManager(VertexArrayObjectPointer);
            Camera = camera;
        }

        public void InitShader(string shader)
        {
            Shader = Resources.Shaders[shader];
        }
        #endregion


        #region Render  

        public virtual void Render(PrimitiveType type = PrimitiveType.Triangles)
        {
            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Shader.SetMatrix4("model", Transform.GetModelMatrix());


            GL.BindVertexArray(VertexArrayObjectPointer);
            GL.DrawArrays(type, 0, size);
        }
        public virtual void Render(uint[] indices, PrimitiveType type = PrimitiveType.Triangles)
        { // deadass render that shit cuh 
            // on it boss ima render that shit cuh

            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Shader.SetMatrix4("model", Transform.GetModelMatrix());

            GL.BindVertexArray(VertexArrayObjectPointer);

            GL.DrawElements(type, indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        #endregion


    }
}
