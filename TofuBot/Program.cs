using BotOneOne.Connectivity;
using BotOneOne.Protocol.OneBot;
using TofuBot.AI.OpenAI;
using TofuBot.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TofuBot;

class Program
{
    private const string ModelUsageRespond = "respond";
    private const string ModelUsagePurposeDetect = "purpose-detect";
    
    private Config _config = null!;
    private ModelPool _modelPool = new();
    
    private void LoadConfig()
    {
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        _config = deserializer.Deserialize<Config>(File.ReadAllText("config.yml"));
    }

    private void BuildModelPool()
    {
        foreach (
            var model in _config.Models
                .Select(x =>
                {
                    var provider = _config.Providers.First(y => y.Name == x.Provider);
                    return new ModelOptions
                    {
                        ModelName = x.ModelName,
                        Usage = x.Usage,
                        Endpoint = provider.Endpoint,
                        SecretKey = provider.SecretKey
                    };
                }))
        {
            _modelPool.AddModel(model);
        }
    }
    
    public void Bootstrap()
    {
        LoadConfig();
        BuildModelPool();
    }
    public static void Main(string[] args)
    {
        new Program().Bootstrap();
        
        var source = new ReversedWebSocketConnectionSource();
        var listener = new ReversedWebSocketListener(source);
        listener.AddPrefix("http://+:65511/");
        listener.WebSocketConnected += () => Console.WriteLine("Connected");
        
        var oneBot = new OneBotV11Context(source);
        
        listener.Start();
        oneBot.Open();

        while (true)
        {
            var cmd = Console.ReadLine();
            // Console.WriteLine("");
        }
    }
}
