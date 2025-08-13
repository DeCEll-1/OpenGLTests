using RGL.API.Attributes;
using RGL.API.JSON;
using RGL.API.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RGL.API
{
    public class ReflectiveSettings
    {
        static JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new Vector2JsonConverter(),
                new Vector2iJsonConverter(),
                new Vector3JsonConverter(),
                new Vector3iJsonConverter(),
                new Vector4JsonConverter(),
                new Vector4iJsonConverter(),
                new DirectoryInfoJsonConverter(),
            }
        };

        public static void Load<T>() where T : ReflectiveSettings
        {
            IEnumerable<PropertyInfo> properties = ReflectionMisc.GetProperties(typeof(T), BindingFlags.Static | BindingFlags.Public);

            string json = File.ReadAllText(GetSettingsFilePath());

            if (json.Length == 0)
            {
                Save<T>();
            }

            string path = GetSettingsFilePath();


            json = File.ReadAllText(path);

            Dictionary<string, JsonElement> values = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)!;


            foreach (PropertyInfo property in properties)
            {

                if (!values.TryGetValue(property.Name, out JsonElement value))
                    continue;

                object? val = JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType, jsonOptions);

                property.SetValue(null, val);
            }
        }

        public static void Save<T>() where T : ReflectiveSettings
        {
            Dictionary<string, object?> valueDictionary = new Dictionary<string, object?>();

            IEnumerable<PropertyInfo> properties = ReflectionMisc.GetProperties(typeof(T), BindingFlags.Static | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {

                // if it haves the do not save we skip
                if (Attribute.IsDefined(property, typeof(DoNotSaveAttribute)))
                    continue;

                valueDictionary[property.Name] = property.GetValue(null, null);
            }

            string path = GetSettingsFilePath();

            string json = JsonSerializer.Serialize(valueDictionary, jsonOptions);

            File.WriteAllText(path, json);

        }
        public static string AppName = "";
        public static string GetSettingsFilePath(string fileName = "settings.json")
        {
            string baseDir;

            if (OperatingSystem.IsWindows())
            {
                baseDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            else if (OperatingSystem.IsLinux())
            {
                baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");
            }
            else if (OperatingSystem.IsMacOS())
            {
                baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support");
            }
            else
            {
                throw new PlatformNotSupportedException("Unknown OS");
            }

            string dir = Path.Combine(baseDir, AppName);
            Directory.CreateDirectory(dir); // ensure it exists
            string filePath = Path.Combine(dir, fileName);
            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            return filePath;
        }

    }
}
