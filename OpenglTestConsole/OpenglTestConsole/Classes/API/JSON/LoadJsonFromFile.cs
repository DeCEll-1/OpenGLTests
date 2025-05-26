namespace OpenglTestConsole.Classes.API.JSON
{
    public class LoadJsonFromFile<T>
        where T : class
    {
        public static T? Load(string path)
        {
            string json = System.IO.File.ReadAllText(path);
            T data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json)!;
            return data;
        }
    }
}
