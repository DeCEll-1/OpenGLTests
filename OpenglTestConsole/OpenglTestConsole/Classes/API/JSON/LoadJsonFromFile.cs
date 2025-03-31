using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.JSON
{
    public class LoadJsonFromFile<T> where T : class
    {
        public static T Load(string path)
        {
            string json = System.IO.File.ReadAllText(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

    }
}
