using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace RGL.API.JSON
{
    public static class JsonUtil
    {

        public static T? LoadFromFile<T>(string path) where T : class
        {
            return null;
            string json = File.ReadAllText(path);


            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings());
        }
    }
}
