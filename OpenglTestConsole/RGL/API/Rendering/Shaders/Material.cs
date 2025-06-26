using RGL.API.SceneFolder;
using RGL.Classes.API.Rendering.Shaders;

namespace RGL.API.Rendering.Shaders
{
    public abstract class Material
    {
        // ok the plan should be easy
        // so i have Mesh but i want to set each mesh to a specific material
        // i can do it in the constructor, but thats meh
        // so ill just use generalised types like Mesh<T> extends material or w/e
        public Material() { }

        public abstract Shader Shader { get; }

        public abstract void Apply(Scene scene);
    }
}
