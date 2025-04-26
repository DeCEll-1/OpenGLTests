using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics.CodeAnalysis;
using OpenglTestConsole.Classes.API.misc;
using OpenglTestConsole.Classes.Paths;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.Rendering.Geometries;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
{
    public partial class Mesh
    {
        #region Init
        private Camera Camera { get => Scene.Camera; }
        // make this geometry updateable with set => update buffer
        public Geometry3D Geometry { get; set; }
        public Material Material { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public BufferManager BufferManager { get; }
        public int VertexArrayObjectPointer { get; private set; }
        public Mesh(Geometry3D geometry, Material material)
        {
            this.Geometry = geometry;
            this.Material = material;

            VertexArrayObjectPointer = GL.GenVertexArray();
            BufferManager = new BufferManager(VertexArrayObjectPointer);
            this.Geometry.Apply(this.BufferManager);
        }

        #endregion


        #region Render  

        public virtual void Render()
        { // deadass render that shit cuh 
          // on it boss ima render that shit cuh

            PrimitiveType type = PrimitiveType.Triangles;

            Material.Shader.Use();

            Material.Apply();

            Material.Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Material.Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Material.Shader.SetMatrix4("model", Transform.GetModelMatrix());

            GL.BindVertexArray(VertexArrayObjectPointer);

            GL.DrawElements(type, Geometry.IndicesLength, DrawElementsType.UnsignedInt, 0);
        }


        #endregion


    }
}
