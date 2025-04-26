using OpenglTestConsole.Classes.API.Rendering.MeshClasses;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    public abstract class Geometry3D
    {
        public int size;
        public int IndicesLength { get => this.Indices.Length; }
        public uint[] Indices = [];

        public Vector3[] Vertices = Array.Empty<Vector3>();
        public Vector3[] Normals = Array.Empty<Vector3>();
        public Vector2[] TexCoords = Array.Empty<Vector2>();

        public virtual void Apply(BufferManager BufferManager)
        {
            size = this.Vertices.Length; // almost forgor this lmao
            BufferManager.SetVector3(this.Vertices, 0);
            BufferManager.SetVector3(this.Normals, 1);
            BufferManager.SetVector2(this.TexCoords, 2);
            BufferManager.SetIndices(this.Indices);
        }
    }
}
