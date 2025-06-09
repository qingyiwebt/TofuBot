namespace BotOneOne.Extensions.NapCat;

public static class NapCatExtensions
{
    public static async Task SendPoke(this OneBotContext context, object target)
    {
        // 这里的 send_poke 是 NapCat 特有的方言
        await context.InvokeCommand<object>("send_poke", null);
    }
}