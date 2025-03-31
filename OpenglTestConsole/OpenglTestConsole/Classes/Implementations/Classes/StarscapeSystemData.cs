using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.Implementations.Classes
{
    public class StarscapeSystemData
    {
        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("spice")]
        public Spice Spice { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("spectralClass")]
        public SpectralClass SpectralClass { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("links")]
        public string[] Links { get; set; }

        [JsonProperty("uncharted")]
        public bool Uncharted { get; set; }

        [JsonProperty("position")]
        public float[] PosArray { set { Position = new(value[0], value[1], value[2]); } }
        public Vector3 Position { get; set; }

        [JsonProperty("warfare")]
        public int Warfare { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("faction")]
        public Faction Faction { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }
    }

    public enum Spice
    {
        Red,
        Blue,
        Green,
        Purple,
        Yellow,
        Orange,
        Silver,
        None
    }
    public enum SpectralClass
    {
        K,
        M,
        B,
        F,
        A,
        G
    }
    public enum Faction
    {
        Neutral,
        Lycentian,
        Foralkan,
        Kavani,
        TradeUnion,
        Syndicate,
        MiningGuild
    }
    public enum Security
    {
        Wild,
        Unsecure,
        Secure,
        Contested,
        Core,
    }
    public enum Region
    {
        Arkana,
        Wildar,
        Core,
        Altera,
        Eternity,
        Sanctum,
        Olivava,
        Beacon,
        Terminus,
    }
}
