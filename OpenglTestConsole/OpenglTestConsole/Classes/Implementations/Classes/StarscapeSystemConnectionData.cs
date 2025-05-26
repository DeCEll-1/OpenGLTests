using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace OpenglTestConsole.Classes.Implementations.Classes
{
    internal class StarscapeSystemConnectionData
    {
        [JsonProperty("center")]
        public float[] centerArray
        {
            set { Center = new(value[0], value[1], value[2]); }
        }
        public Vector3 Center { get; set; }

        [JsonProperty("direction")]
        public float[] directionArray
        {
            set { Direction = new(value[0], value[1], value[2]); }
        }
        public Vector3 Direction { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }
    }
}
