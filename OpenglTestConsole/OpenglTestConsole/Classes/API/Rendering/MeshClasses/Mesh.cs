﻿using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Shaders;
using OpenglTestConsole.Classes.API.SceneFolder;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
{
    public partial class Mesh
    {
        #region Init
        internal Camera Camera { get => Scene.Camera; }
        public string Name { get; set; }

        internal Geometry3D _geometry;
        public Geometry3D Geometry { get => _geometry; set { _geometry = value; _geometry.Apply(this.BufferManager); } }
        public Material Material { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public BufferManager BufferManager { get; internal set; }

        // caps to enable before rendering
        public List<EnableCap> CapsToEnable { get; set; } = new();

        // caps to disable before rendering
        public List<EnableCap> CapsToDisable { get; set; } = new();

        public Action BeforeRender;

        public Action AfterRender;
        // render type
        public PrimitiveType type { get; set; } = PrimitiveType.Triangles;

        public int VertexArrayObjectPointer { get; internal set; }

        public Mesh(Geometry3D geometry, Material material, string name = "")
        {
            this.Name = (name == "") ? Guid.NewGuid().ToString() : name;
            this._geometry = geometry;
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

            Enalbes();


            Material.Shader.Use();

            Material.Apply();

            Material.Shader.UniformManager.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Material.Shader.UniformManager.SetMatrix4("view", Camera.GetViewMatrix());

            Material.Shader.UniformManager.SetMatrix4("model", Transform.GetModelMatrix());


            GL.BindVertexArray(VertexArrayObjectPointer);
            this.BeforeRender?.Invoke();
            if (Geometry.IndicesLength < 3) // check if we are using indices
                GL.DrawArrays(type, 0, Geometry.VerticesLength);
            else
                GL.DrawElements(type, Geometry.IndicesLength, DrawElementsType.UnsignedInt, 0);
            this.AfterRender?.Invoke();

            Disables();
        }

        private void Enalbes()
        {
            foreach (EnableCap cap in CapsToDisable)
                GL.Disable(cap);

            foreach (EnableCap cap in CapsToEnable)
                GL.Enable(cap);
        }

        private void Disables()
        {
            foreach (EnableCap cap in CapsToDisable)
                GL.Enable(cap);

            foreach (EnableCap cap in CapsToEnable)
                GL.Disable(cap);
        }

        #endregion
    }
}
