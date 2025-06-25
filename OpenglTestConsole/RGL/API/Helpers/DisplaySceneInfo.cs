using ICSharpCode.Decompiler.CSharp.Syntax;
using ImGuiNET;
using RGL.Classes.API;
using OpenTK.Mathematics;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using Scene = RGL.API.SceneFolder.Scene;
using RGL.API.Rendering;
using RGL.API.Rendering.MeshClasses;
using RGL.API.Misc;

namespace RGL.Classes.Implementations.RenderScripts
{
    public class DisplaySceneInfo : RenderScript
    {
        public override void Init() { }
        public override void Advance()
        {
            ImGui.Begin("Scene Info");

            ImguiMisc.RenderSceneDebugInfo(Scene);

            ImGui.End();

            

        }



    }
}
