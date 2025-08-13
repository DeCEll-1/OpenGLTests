using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.JSON
{
    public class NewtonsoftVector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector2((float)arr[0], (float)arr[1]);
        }

        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftVector2iJsonConverter : JsonConverter<Vector2i>
    {
        public override Vector2i ReadJson(JsonReader reader, Type objectType, Vector2i existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector2i((int)arr[0], (int)arr[1]);
        }

        public override void WriteJson(JsonWriter writer, Vector2i value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftVector3JsonConverter : JsonConverter<Vector3>
    {
        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector3((float)arr[0], (float)arr[1], (float)arr[2]);
        }

        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftVector3iJsonConverter : JsonConverter<Vector3i>
    {
        public override Vector3i ReadJson(JsonReader reader, Type objectType, Vector3i existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector3i((int)arr[0], (int)arr[1], (int)arr[2]);
        }

        public override void WriteJson(JsonWriter writer, Vector3i value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftVector4JsonConverter : JsonConverter<Vector4>
    {
        public override Vector4 ReadJson(JsonReader reader, Type objectType, Vector4 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector4((float)arr[0], (float)arr[1], (float)arr[2], (float)arr[3]);
        }

        public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteValue(value.W);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftVector4iJsonConverter : JsonConverter<Vector4i>
    {
        public override Vector4i ReadJson(JsonReader reader, Type objectType, Vector4i existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray arr = JArray.Load(reader);
            return new Vector4i((int)arr[0], (int)arr[1], (int)arr[2], (int)arr[3]);
        }

        public override void WriteJson(JsonWriter writer, Vector4i value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteValue(value.W);
            writer.WriteEndArray();
        }
    }

    public class NewtonsoftDirectoryInfoJsonConverter : JsonConverter<DirectoryInfo>
    {
        public override DirectoryInfo ReadJson(JsonReader reader, Type objectType, DirectoryInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string val = (string)JToken.Load(reader);
            return new DirectoryInfo(val);
        }

        public override void WriteJson(JsonWriter writer, DirectoryInfo value, JsonSerializer serializer)
        {
            writer.WriteValue(value.FullName);
        }
    }
}
