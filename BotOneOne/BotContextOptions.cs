namespace BotOneOne;

public struct BotContextOptions
{
    public TimeSpan InvocationTimeout { get; set; }

    public static BotContextOptions Default => new()
    {
        InvocationTimeout = TimeSpan.FromSeconds(30)
    };
}