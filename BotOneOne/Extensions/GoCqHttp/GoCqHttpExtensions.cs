namespace BotOneOne.Extensions.GoCqHttp;

public static class GoCqHttpExtensions
{
    public static async Task SetQQProfile(this OneBotContext context, object payload)
    {
        // 这里是 go-cqhttp 的方言
        await context.InvokeCommand<object>("set_qq_profile", payload);
    } 
}