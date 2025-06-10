using BotOneOne;
using BotOneOne.Connectivity;
using BotOneOne.Extensions.OneBotV11;
using TofuBot.AI.VectorDB;

namespace TofuBot;

public class BotContext
{
    private IConnectionSource _connectionSource;
    private ReversedWebSocketListener _webSocketListener;
    private OneBotContext _oneBotContext;

    private VectorDbContext _vectorDbContext;
    
    public BotContext()
    {
        // Bot side
        var source = new ReversedWebSocketConnectionSource();
        var listener = new ReversedWebSocketListener(source);
        listener.AddPrefix("http://*:11122/");
        listener.WebSocketConnected += () => Console.WriteLine("[Protocol Server] connected");
        
        var oneBot = new OneBotContext(source);
        oneBot.UseOneBotV11();
        
        _connectionSource = source;
        _webSocketListener = listener;
        _oneBotContext = oneBot;
        
        // AI side
        _vectorDbContext = new VectorDbContext();
    }

    public void Start()
    {
        _webSocketListener.Start();
        _oneBotContext.Open();
    }
}