using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.API.Rendering
{
    public class Light
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Color = Vector3.Zero;

        public Light(Vector3 location, Vector3 color)
        {
            Position = location;
            Color = color;
        }

        public Vector3 Specular
        {
            get => Color;
        }
        public Vector3 Diffuse
        {
            get => Color * 0.7f;
        }
        public Vector3 Ambient
        {
            get => Color * 0.4f;
        }
    }
}
