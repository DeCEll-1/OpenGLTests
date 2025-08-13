using ICSharpCode.Decompiler.CSharp.Syntax;
using ImGuiNET;
using RGL.API.Rendering;
using RGL.API.SceneFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RGL.API.ImGuiHelpers
{
    internal class Notification
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public double Duration { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public bool IsExpired()
        {
            return (DateTime.UtcNow - StartTime).TotalMilliseconds > Duration;
        }
    }
    public class ImguiNotification
    {
        private static List<Notification> Notifications = new();
        public static void DisplayNotification(string title, string text, double duration)
        {
            Notifications.Add(new Notification() { Title = title, Text = text, Duration = duration });
        }

        public static void RenderNotifications()
        {
            Notifications = Notifications.FindAll(notif =>
            {
                bool returnVal = notif.IsExpired();
                return !returnVal;
            });


            ImGui.Begin("Notifications");
            foreach (var notif in Notifications.ToList())
            {
                ImGui.Text(notif.Title);
                ImGui.Indent();
                ImGui.NewLine();
                RenderAnsiText(notif.Text);
                ImGui.Unindent();
            }

            ImGui.SetScrollHereY(1f);
            ImGui.End();
        }

        private static readonly Dictionary<int, System.Numerics.Vector4> AnsiColorToImGui = new Dictionary<int, System.Numerics.Vector4>
        {
            [30] = new System.Numerics.Vector4(0, 0, 0, 1),           // Black
            [31] = new System.Numerics.Vector4(0.8f, 0.1f, 0, 1),     // Red
            [32] = new System.Numerics.Vector4(0, 0.7f, 0, 1),        // Green
            [33] = new System.Numerics.Vector4(0.8f, 0.7f, 0, 1),     // Yellow
            [34] = new System.Numerics.Vector4(0, 0.5f, 1, 1),        // Blue
            [35] = new System.Numerics.Vector4(0.8f, 0, 0.8f, 1),     // Magenta
            [36] = new System.Numerics.Vector4(0, 0.7f, 0.7f, 1),     // Cyan
            [37] = new System.Numerics.Vector4(1, 1, 1, 1),           // White
            [90] = new System.Numerics.Vector4(0.4f, 0.4f, 0.4f, 1),  // Bright Black (Gray)
            [91] = new System.Numerics.Vector4(1, 0.3f, 0.3f, 1),     // Bright Red
            [92] = new System.Numerics.Vector4(0.3f, 1, 0.3f, 1),     // Bright Green
            [93] = new System.Numerics.Vector4(1, 1, 0.3f, 1),        // Bright Yellow
            [94] = new System.Numerics.Vector4(0.3f, 0.6f, 1, 1),     // Bright Blue
            [95] = new System.Numerics.Vector4(1, 0.3f, 1, 1),        // Bright Magenta
            [96] = new System.Numerics.Vector4(0.3f, 1, 1, 1),        // Bright Cyan
            [97] = new System.Numerics.Vector4(1, 1, 1, 1),           // Bright White
        };
        // Regex to match ANSI escape codes (e.g., "\x1b[31m")
        private static readonly Regex AnsiRegex = new Regex(@"(\x1b\[(\d+)m)", RegexOptions.Compiled);
        public static void RenderAnsiText(string input)
        {
            int lastIndex = 0;
            System.Numerics.Vector4? currentColor = null;

            foreach (Match match in AnsiRegex.Matches(input))
            {
                int start = match.Index;
                int length = match.Length;

                // Print text before this escape code
                if (start > lastIndex)
                {
                    string text = input.Substring(lastIndex, start - lastIndex);
                    RenderSegment(text, currentColor);
                }

                // Update color based on the escape code
                if (int.TryParse(match.Groups[2].Value, out int colorCode))
                {
                    // Reset color
                    if (colorCode == 0)
                    {
                        currentColor = null;
                    }
                    else if (AnsiColorToImGui.TryGetValue(colorCode, out var color))
                    {
                        currentColor = color;
                    }
                }

                lastIndex = start + length;
            }

            // Render any remaining text
            if (lastIndex < input.Length)
            {
                string text = input.Substring(lastIndex);
                RenderSegment(text, currentColor);
            }
        }
        private static void RenderSegment(string text, System.Numerics.Vector4? color)
        {
            if (string.IsNullOrEmpty(text)) return;


            string[] lines = text.Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                ImGui.SameLine(0, 0); // Continue on the same line for next segment
                string? line = lines[i];
                if (color.HasValue)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, color.Value);
                    ImGui.TextUnformatted(line);
                    ImGui.PopStyleColor();
                }
                else
                {
                    ImGui.TextUnformatted(line);
                }
                if (i + 1 != lines.Length)
                {
                    ImGui.NewLine();
                }

            }
            //ImGui.SameLine(0, 0); // Continue on the same line for next segment
        }

    }
}
