using RGL.API.Rendering.Shaders;
using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;

namespace RGL.API.Rendering.Materials
{
    internal class StandartMaterial : Material
    {
        public bool Wireframe { get; set; }
        public bool FlatShaded { get; set; }

        public override Shader Shader => throw new NotImplementedException();

        public StandartMaterial() { }
        public override void Apply(Scene scene)
        {
            // add shader sets here

        }
    }
}
