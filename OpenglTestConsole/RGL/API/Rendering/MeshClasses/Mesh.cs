using RGL.API.Rendering.Geometries;
using RGL.API.Rendering.Shaders;
using RGL.API.SceneFolder;

namespace RGL.API.Rendering.MeshClasses
{
    public partial class Mesh
    {
        #region Init
        public string Name { get; set; }

        internal Geometry3D _geometry;
        public Geometry3D Geometry { get => _geometry; set { _geometry = value; _geometry.Apply(BufferManager); } }
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
        public bool IsTransparent
        {
            get; set
            {
                field = value;
                this.Material.Transparent = value;
            }
        } = false;

        public int VertexArrayObjectPointer { get; internal set; }

        public Mesh(Geometry3D geometry, Material material, string name = "")
        {
            Name = name == "" ? Guid.NewGuid().ToString() : name;
            _geometry = geometry;
            Material = material;

            VertexArrayObjectPointer = GL.GenVertexArray();
            BufferManager = new BufferManager(VertexArrayObjectPointer);
            Geometry.Apply(BufferManager);
        }
        #endregion


        #region Render

        public virtual void Render(Scene scene, bool? Transparent = null)
        { // deadass render that shit cuh
            // on it boss ima render that shit cuh

            bool realTransparency = this.IsTransparent;
            this.IsTransparent = Transparent ?? this.IsTransparent;

            Enalbes();

            Material.BeforeUse(scene);

            Material.Shader.Use();

            Material.Apply(scene);


            Material.Shader.UniformManager.SetMatrix4("projection", scene.Camera.GetProjectionMatrix());

            Material.Shader.UniformManager.SetMatrix4("view", scene.Camera.GetViewMatrix());

            Material.Shader.UniformManager.SetMatrix4("model", Transform.GetModelMatrix());


            GL.BindVertexArray(VertexArrayObjectPointer);
            BeforeRender?.Invoke();
            if (Geometry.IndicesLength < 3) // check if we are using indices
                GL.DrawArrays(type, 0, Geometry.VerticesLength);
            else
                GL.DrawElements(type, Geometry.IndicesLength, DrawElementsType.UnsignedInt, 0);
            AfterRender?.Invoke();

            Disables();
            this.IsTransparent = realTransparency;
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
