using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Rendering.Geometries
{
    public class SkyboxGeometry : Geometry3D
    {
        public SkyboxGeometry()
        {
            Init();
        }
        private void Init()
        {
            (this.Vertices, this.Normals, this.TexCoords, this.Indices) = GetSkybox();
        }

        private (
            Vector3[] vertices,
            Vector3[] normals,
            Vector2[] texCoords,
            uint[] indices
        ) GetSkybox()
        {
            return (
                vertices:
                [
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),

                    new(-1.0f, -1.0f,  1.0f),
                    new(-1.0f, -1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f, -1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new(-1.0f, -1.0f,  1.0f),

                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),

                    new(-1.0f, -1.0f,  1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f, -1.0f,  1.0f),
                    new(-1.0f, -1.0f,  1.0f),

                    new(-1.0f,  1.0f, -1.0f),
                    new( 1.0f,  1.0f, -1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new( 1.0f,  1.0f,  1.0f),
                    new(-1.0f,  1.0f,  1.0f),
                    new(-1.0f,  1.0f, -1.0f),

                    new(-1.0f, -1.0f, -1.0f),
                    new(-1.0f, -1.0f,  1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new( 1.0f, -1.0f, -1.0f),
                    new(-1.0f, -1.0f,  1.0f),
                    new( 1.0f, -1.0f,  1.0f)
                ],
                normals: [],
                texCoords: [], // same as the vertex positions
                indices: []
                );
        }
    }
}
