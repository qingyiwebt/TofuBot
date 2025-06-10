namespace BotOneOne;

public struct OneBotContextOptions
{
    public TimeSpan Timeout;

    public static OneBotContextOptions Default => new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };
}