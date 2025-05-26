using OpenglTestConsole.Classes.API.Rendering.Geometries;
using OpenglTestConsole.Classes.API.Rendering.Shaders;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
{
    public partial class Mesh
    {
        #region Init
        private Camera Camera
        {
            get => Scene.Camera;
        }

        private Geometry3D _geometry;
        public Geometry3D Geometry { get => _geometry; set { _geometry = value; _geometry.Apply(this.BufferManager); } }
        public Material Material { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public BufferManager BufferManager { get; }

        // caps to enable before rendering
        public List<EnableCap> CapsToEnable { get; set; } = new();

        // caps to disable before rendering
        public List<EnableCap> CapsToDisable { get; set; } = new();
        public int VertexArrayObjectPointer { get; private set; }

        public Mesh(Geometry3D geometry, Material material)
        {
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

            PrimitiveType type = PrimitiveType.Triangles;

            Material.Shader.Use();

            Material.Apply();

            Material.Shader.UniformManager.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Material.Shader.UniformManager.SetMatrix4("view", Camera.GetViewMatrix());

            Material.Shader.UniformManager.SetMatrix4("model", Transform.GetModelMatrix());


            GL.BindVertexArray(VertexArrayObjectPointer);

            GL.DrawElements(type, Geometry.IndicesLength, DrawElementsType.UnsignedInt, 0);

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
