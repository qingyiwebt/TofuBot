using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotOneOne.MessageFormat;

public class UnknownMessageSegment : MessageSegment
{
    [JsonProperty("type")] public string RawType { get; set; }
    [JsonIgnore] public override string Type => RawType;
    [JsonExtensionData] public Dictionary<string, JToken> ExtensionData { get; set; } = [];

    public override string ToString()
    {
        return $"[Unknown type {RawType}]";
    }
}