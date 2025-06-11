using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotOneOne.MessageFormat;

public class Message
{
    public static Message Empty { get; } = new();

    public List<MessageSegment> Segments { get; } = [];

    public Message Append(MessageSegment segment)
    {
        Segments.Add(segment);
        return this;
    }

    public Message Append(Message other)
    {
        Segments.AddRange(other.Segments);
        return this;
    }

    public override string ToString()
    {
        return string.Join(' ', Segments.Select(x => x.ToString()));
    }

    private static readonly Dictionary<string, Type> SegmentTypes = new()
    {
        { "text", typeof(TextMessageSegment) },
        { "at", typeof(AtMessageSegment) },
        { "image", typeof(ImageMessageSegment) },
        { "reply", typeof(ReplyMessageSegment) }
    };

    public static Message Parse(JArray array)
    {
        var result = new Message();
        foreach (var item in array)
        {
            MessageSegment? segment;
            if (SegmentTypes.TryGetValue(item["type"]?.ToString() ?? string.Empty, out var type))
            {
                segment = item.ToObject(type) as MessageSegment;
            } else
            {
                segment = item.ToObject<UnknownMessageSegment>();
            }

            segment ??= segment ??
                        throw new NullReferenceException("Unexpected null when parsing message");

            result.Append(segment);
        }

        return result;
    }

    public static Message Parse(string str)
    {
        return Parse(JArray.Parse(str));
    }
}
