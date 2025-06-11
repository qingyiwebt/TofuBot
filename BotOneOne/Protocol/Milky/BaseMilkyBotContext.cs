namespace BotOneOne.Protocol.Milky;

public abstract class BaseMilkyBotContext : IBotContext
{
    public bool IsOpened { get; }
    public void Open()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public Task InvokeAction<T>(string actionName, T? parameters)
    {
        throw new NotImplementedException();
    }

    public Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters)
    {
        throw new NotImplementedException();
    }
}