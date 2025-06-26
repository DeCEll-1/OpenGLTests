using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.JSON;

using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        float[] arr = JsonSerializer.Deserialize<float[]>(ref reader, options)!;
        return new Vector2(arr[0], arr[1]);
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y }, options);
    }
}

public class Vector2iJsonConverter : JsonConverter<Vector2i>
{
    public override Vector2i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int[] arr = JsonSerializer.Deserialize<int[]>(ref reader, options)!;
        return new Vector2i(arr[0], arr[1]);
    }

    public override void Write(Utf8JsonWriter writer, Vector2i value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y }, options);
    }
}

public class Vector3JsonConverter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        float[] arr = JsonSerializer.Deserialize<float[]>(ref reader, options)!;
        return new Vector3(arr[0], arr[1], arr[2]);
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y, value.Z }, options);
    }
}

public class Vector3iJsonConverter : JsonConverter<Vector3i>
{
    public override Vector3i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int[] arr = JsonSerializer.Deserialize<int[]>(ref reader, options)!;
        return new Vector3i(arr[0], arr[1], arr[2]);
    }

    public override void Write(Utf8JsonWriter writer, Vector3i value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y, value.Z }, options);
    }
}

public class Vector4JsonConverter : JsonConverter<Vector4>
{
    public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        float[] arr = JsonSerializer.Deserialize<float[]>(ref reader, options)!;
        return new Vector4(arr[0], arr[1], arr[2], arr[3]);
    }

    public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y, value.Z, value.W }, options);
    }
}

public class Vector4iJsonConverter : JsonConverter<Vector4i>
{
    public override Vector4i Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int[] arr = JsonSerializer.Deserialize<int[]>(ref reader, options)!;
        return new Vector4i(arr[0], arr[1], arr[2], arr[3]);
    }

    public override void Write(Utf8JsonWriter writer, Vector4i value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new[] { value.X, value.Y, value.Z, value.W }, options);
    }
}

