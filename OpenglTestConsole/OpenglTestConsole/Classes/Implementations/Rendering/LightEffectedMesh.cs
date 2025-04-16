using OpenglTestConsole.Classes;
using OpenglTestConsole.Classes.api.rendering;
using OpenglTestConsole.Classes.API.Rendering.Mesh;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Rendering
{
    public abstract class LightEffectedMesh : Mesh
    {
        [SetsRequiredMembers]
        protected LightEffectedMesh(Camera camera) : base(camera) { }

        // TODO: make this take a light array or list instead of a singular light cuz, we can, yk, use more lights
        public void SetStaticUniforms(Light light)
        {
            Shader.SetVector3("lightPos", light.Location);
            Shader.SetVector4("lightColorIn", light.Color);
            Shader.SetVector4("ambientIn", light.Ambient);

            Shader.SetVector3("viewPos", Camera.Position);
        }
        public virtual void PrepareRender(Light light)
        {
            this.Shader.Use();
            this.SetStaticUniforms(light);
            GL.Enable(EnableCap.CullFace); // so that it doesnt render the back side
        }
        public virtual void EndRender() => GL.Disable(EnableCap.CullFace);
    }
}
