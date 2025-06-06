using ImGuiNET;
using OpenglTestConsole.Classes.API.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.RenderScripts
{
    public class WindowSizeSettings : RenderScript
    {
        public override void Init()
        {
        }

        public override void Advance()
        {
            ImGui.Begin("Window Settings");
            ImGui.Text("Current Resolution: " + Program.main.ClientSize.ToString());

            ImGui.Text("16:9");

            ImGui.SameLine();
            if (ImGui.Button("3840x2160"))
                Program.main.ClientSize = new(3840, 2160);

            ImGui.SameLine();
            if (ImGui.Button("2560x1440"))
                Program.main.ClientSize = new(2560, 1440);

            ImGui.SameLine();
            if (ImGui.Button("1920x1080"))
                Program.main.ClientSize = new(1920, 1080);

            ImGui.SameLine();
            if (ImGui.Button("1600x900"))
                Program.main.ClientSize = new(1600, 900);

            ImGui.SameLine();
            if (ImGui.Button("1366x768"))
                Program.main.ClientSize = new(1366, 768);

            ImGui.SameLine();
            if (ImGui.Button("1280x720"))
                Program.main.ClientSize = new(1280, 720);

            ImGui.NewLine();
            ImGui.Text("4:3");

            ImGui.SameLine();
            if (ImGui.Button("1600x1200"))
                Program.main.ClientSize = new(1600, 1200);

            ImGui.SameLine();
            if (ImGui.Button("1280x960"))
                Program.main.ClientSize = new(1280, 960);

            ImGui.SameLine();
            if (ImGui.Button("1024x768"))
                Program.main.ClientSize = new(1024, 768);

            ImGui.SameLine();
            if (ImGui.Button("800x600"))
                Program.main.ClientSize = new(800, 600);

            ImGui.SameLine();
            if (ImGui.Button("640x480"))
                Program.main.ClientSize = new(640, 480);

            ImGui.NewLine();
            ImGui.Text("16:10");

            ImGui.SameLine();
            if (ImGui.Button("1920x1200"))
                Program.main.ClientSize = new(1920, 1200);

            ImGui.SameLine();
            if (ImGui.Button("1680x1050"))
                Program.main.ClientSize = new(1680, 1050);

            ImGui.SameLine();
            if (ImGui.Button("1440x900"))
                Program.main.ClientSize = new(1440, 900);

            ImGui.SameLine();
            if (ImGui.Button("1280x800"))
                Program.main.ClientSize = new(1280, 800);

            ImGui.NewLine();
            ImGui.Text("21:9");

            ImGui.SameLine();
            if (ImGui.Button("5120x2160"))
                Program.main.ClientSize = new(5120, 2160);

            ImGui.SameLine();
            if (ImGui.Button("3440x1440"))
                Program.main.ClientSize = new(3440, 1440);

            ImGui.SameLine();
            if (ImGui.Button("2560x1080"))
                Program.main.ClientSize = new(2560, 1080);

            ImGui.NewLine();
            ImGui.Text("5:4");

            ImGui.SameLine();
            if (ImGui.Button("1280x1024"))
                Program.main.ClientSize = new(1280, 1024);

            ImGui.NewLine();
            ImGui.Text("3:2");

            ImGui.SameLine();
            if (ImGui.Button("2160x1440"))
                Program.main.ClientSize = new(2160, 1440);

            ImGui.SameLine();
            if (ImGui.Button("1440x960"))
                Program.main.ClientSize = new(1440, 960);

            ImGui.SameLine();
            if (ImGui.Button("1280x854"))
                Program.main.ClientSize = new(1280, 854);


            ImGui.End();
        }

    }
}
