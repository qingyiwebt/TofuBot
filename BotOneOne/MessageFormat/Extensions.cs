namespace BotOneOne.MessageFormat;

public static class Extensions
{
    public static Message Text(this Message message, string content)
    {
        message.Append(new TextMessageSegment(content));
        return message;
    }

    public static Message At(this Message message, long target)
    {
        message.Append(new AtMessageSegment(target));
        return message;
    }
}