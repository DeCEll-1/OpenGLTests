using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics.CodeAnalysis;

namespace OpenglTestConsole.classes
{
    public class Mesh
    {
        #region Init
        /// <summary>
        /// The corner amount of the mesh
        /// </summary>
        public int size { get; set; }
        public required Camera2D Camera { get; set; } = new Camera2D(800, 600);
        public required Shader Shader { get; set; } = new Shader("shaders/default.vert", "shaders/default.frag");
        public Transform Transform { get; set; } = new Transform();
        private int VertexArrayObjectPointer { get; set; }
        [SetsRequiredMembers]
        public Mesh(Camera2D camera, int size = 3, string vert = "", string frag = "")
        {
            this.size = size;
            if (vert != "" || frag != "")
                InitShader(vert, frag);
            VertexArrayObjectPointer = GL.GenVertexArray();
            Camera = camera;

        }
        #endregion

        #region Shader
        public void InitShader(string vertLoc, string fragLoc)
        {
            Shader = new Shader(vertLoc, fragLoc);
        }
        public void SetVector2(Vector2[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer

            GL.VertexAttribPointer(loc, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetVector3(Vector3[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer

            GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetVector4(Vector4[] vectors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector4.SizeInBytes, vectors, BufferUsageHint.StaticDraw); // put data in buffer


            GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes, 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetColor(Color4[] colors, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * 4 * sizeof(float), colors, BufferUsageHint.StaticDraw); // put data in buffer


            GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0); // bind the buffer to location 0

            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        public void SetMatrix4(Matrix4[] matrices, int loc)
        {
            GL.BindVertexArray(VertexArrayObjectPointer); // bind the vertex array so that the buffer we made is used on this

            // generate vertex buffer object
            int VBOPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOPointer); // bind buffer
            GL.BufferData(BufferTarget.ArrayBuffer, matrices.Length * 16 * sizeof(float), matrices, BufferUsageHint.StaticDraw); // put data in buffer
            GL.VertexAttribPointer(loc, 16, VertexAttribPointerType.Float, false, 16 * sizeof(float), 0); // bind the buffer to location 0
            GL.EnableVertexAttribArray(loc); // enable loc 0
        }
        #endregion

        #region Render  
        public void Render(PrimitiveType type = PrimitiveType.TriangleStrip)
        {

            Shader.SetMatrix4("view", Camera.GetViewMatrix());

            Shader.SetMatrix4("projection", Camera.GetProjectionMatrix());

            Shader.SetMatrix4("model", Transform.GetModelMatrix());

            GL.BindVertexArray(VertexArrayObjectPointer);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, size);
        }
        #endregion


    }
}
