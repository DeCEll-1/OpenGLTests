using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RGL.API.Misc
{
    public class JsonMisc
    {
        public static string RemoveHashComments(string json)
        {
            var lines = json.Split('\n');
            string cleaned = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                Match match = Regex.Match(line.TrimStart(), @"#.*$");

                if (match.Success)
                    cleaned += line.Substring(0, match.Index + 1) + "\n";
                else
                    cleaned += line;
            }
            return cleaned;
        }

    }
}
