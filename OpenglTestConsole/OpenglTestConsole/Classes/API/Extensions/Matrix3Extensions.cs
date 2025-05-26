using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Extensions
{
    public static class MatrixExtensions
    {
        // for god knows why using instanced rendering (or more so, passing the matrix as a layout) fucks up the rotation
        public static Matrix4 RotateMatrixForInstancedRendering(this Matrix4 m)
        {
            return new Matrix4(
                m.Row0.X,
                m.Row1.X,
                m.Row2.X,
                m.Row3.X,
                m.Row0.Y,
                m.Row1.Y,
                m.Row2.Y,
                m.Row3.Y,
                m.Row0.Z,
                m.Row1.Z,
                m.Row2.Z,
                m.Row3.Z,
                m.Row0.W,
                m.Row1.W,
                m.Row2.W,
                m.Row3.W
            );
        }
    }
}
