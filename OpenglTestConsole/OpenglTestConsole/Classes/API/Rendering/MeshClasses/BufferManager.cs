using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering.MeshClasses
{
    public partial class BufferManager
    {
        #region Shader
        private Dictionary<int, int> _vboCache = new(); // loc -> VBO

        private int VertexArrayObjectPointer;
        private int _eboCache = -1; // Cache for the element buffer object (EBO)

        public BufferManager(int vao)
        {
            VertexArrayObjectPointer = vao;
        }

        public void SetVector2(
            Vector2[] vectors,
            int loc,
            BufferUsageHint bufferHint = BufferUsageHint.StaticDraw
        )
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (!_vboCache.TryGetValue(loc, out int vbo))
            {
                vbo = GL.GenBuffer();
                _vboCache[loc] = vbo;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    vectors.Length * Vector2.SizeInBytes,
                    vectors,
                    bufferHint
                );

                GL.VertexAttribPointer(
                    loc,
                    2,
                    VertexAttribPointerType.Float,
                    false,
                    Vector2.SizeInBytes,
                    0
                );
                GL.EnableVertexAttribArray(loc);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferSubData(
                    BufferTarget.ArrayBuffer,
                    IntPtr.Zero,
                    vectors.Length * Vector2.SizeInBytes,
                    vectors
                );
            }
        }

        public void SetVector3(
            Vector3[] vectors,
            int loc,
            BufferUsageHint bufferHint = BufferUsageHint.StaticDraw
        )
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (!_vboCache.TryGetValue(loc, out int vbo))
            {
                vbo = GL.GenBuffer();
                _vboCache[loc] = vbo;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    vectors.Length * Vector3.SizeInBytes,
                    vectors,
                    bufferHint
                );

                GL.VertexAttribPointer(
                    loc,
                    3,
                    VertexAttribPointerType.Float,
                    false,
                    Vector3.SizeInBytes,
                    0
                );
                GL.EnableVertexAttribArray(loc);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferSubData(
                    BufferTarget.ArrayBuffer,
                    IntPtr.Zero,
                    vectors.Length * Vector3.SizeInBytes,
                    vectors
                );
            }
        }

        public void SetVector4(
            Vector4[] vectors,
            int loc,
            BufferUsageHint bufferHint = BufferUsageHint.StaticDraw
        )
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (!_vboCache.TryGetValue(loc, out int vbo))
            {
                vbo = GL.GenBuffer();
                _vboCache[loc] = vbo;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    vectors.Length * Vector4.SizeInBytes,
                    vectors,
                    bufferHint
                );

                GL.VertexAttribPointer(
                    loc,
                    4,
                    VertexAttribPointerType.Float,
                    false,
                    Vector4.SizeInBytes,
                    0
                );
                GL.EnableVertexAttribArray(loc);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferSubData(
                    BufferTarget.ArrayBuffer,
                    IntPtr.Zero,
                    vectors.Length * Vector4.SizeInBytes,
                    vectors
                );
            }
        }

        public void SetColor(
            Color4[] colors,
            int loc,
            BufferUsageHint bufferHint = BufferUsageHint.StaticDraw
        )
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (!_vboCache.TryGetValue(loc, out int vbo))
            {
                vbo = GL.GenBuffer();
                _vboCache[loc] = vbo;

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    colors.Length * 4 * sizeof(float),
                    colors,
                    bufferHint
                );

                GL.VertexAttribPointer(
                    loc,
                    4,
                    VertexAttribPointerType.Float,
                    false,
                    4 * sizeof(float),
                    0
                );
                GL.EnableVertexAttribArray(loc);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferSubData(
                    BufferTarget.ArrayBuffer,
                    IntPtr.Zero,
                    colors.Length * 4 * sizeof(float),
                    colors
                );
            }
        }

        public void SetIndices(
            uint[] indices,
            BufferUsageHint bufferHint = BufferUsageHint.StaticDraw
        )
        {
            GL.BindVertexArray(VertexArrayObjectPointer);

            if (_eboCache == -1)
            {
                _eboCache = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _eboCache);
                GL.BufferData(
                    BufferTarget.ElementArrayBuffer,
                    indices.Length * sizeof(uint),
                    indices,
                    bufferHint
                );
            }
            else
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _eboCache);
                GL.BufferSubData(
                    BufferTarget.ElementArrayBuffer,
                    IntPtr.Zero,
                    indices.Length * sizeof(uint),
                    indices
                );
            }
        }

        public void Dispose()
        {
            // Delete all VBOs
            foreach (var vbo in _vboCache.Values)
            {
                GL.DeleteBuffer(vbo);
            }

            // Delete the EBO if it exists
            if (_eboCache != -1)
            {
                GL.DeleteBuffer(_eboCache);
            }

            // Clear the cache
            _vboCache.Clear();
            _eboCache = -1;
        }

        #endregion
    }
}
