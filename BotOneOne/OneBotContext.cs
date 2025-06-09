using BotOneOne.Connectivity;

namespace BotOneOne;

public class OneBotContext
{
    private readonly IConnectionSource _connectionSource;

    public OneBotContext(IConnectionSource connectionSource)
    {
        _connectionSource = connectionSource;
    }
    
    public async Task<T> InvokeCommand<T>(string command, object payload)
    {
        throw new NotImplementedException();
    }
}
