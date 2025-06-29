using Newtonsoft.Json;

namespace BotOneOne.MessageFormat;

public class AtMessageSegment : MessageSegment<AtMessageSegment.Payload>
{
    public override string ToString()
    {
        return $"[At {Data.Target}]";
    }

    public AtMessageSegment(long target)
    {
        Data = new Payload
        {
            Target = target
        };
    }
    public struct Payload
    {
        [JsonProperty("qq")] public long Target { get; set; }
    }

    public override string Type => "at";
}