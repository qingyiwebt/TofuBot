using BotOneOne.Connectivity;
using BotOneOne.Protocol.OneBot;
using Microsoft.Extensions.DependencyInjection;
using TofuBot.Abilities;
using TofuBot.AI.OpenAI;
using TofuBot.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TofuBot;

class Program
{
    private Config _config = null!;
    private readonly ServiceProvider _serviceProvider;

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
        var modelPool = _serviceProvider.GetService<ModelPool>()!;
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
                        SecretKey = provider.SecretKey,
                        Temperature = x.Temperature,
                        TopP = x.TopP
                    };
                }))
        {
            modelPool.AddModel(model);
        }
    }
    
    private void BuildBotContext()
    {
        var botContext = _serviceProvider.GetService<CharacterContext>()!;
        botContext.Name = _config.Character.Name;
        botContext.Description = _config.Character.Description;
        botContext.PurposeGeneratePrompt = _config.Prompts.PurposeGenerate;
        
        var abilitiesManager = botContext.AbilitiesManager;
        abilitiesManager.Add(new BrowsingGitHubAbility());
        abilitiesManager.Add(new QQChatAbility());
    }

    private void Bootstrap()
    {
        LoadConfig();
        BuildModelPool();
        BuildBotContext();
        EventLoop();
    }

    private async void EventLoop()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(_config.EventConfig.EventDuration));
            // TODO
        }
    }

    private Program()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<CharacterContext>()
            .AddSingleton<ModelPool>()
            .BuildServiceProvider();
    }

    public static async Task<int> Main(string[] args)
    {
        var program = new Program();
        program.Bootstrap();
        
        while (true)
        {
            Console.Write("Tofu > ");
            var cmd = Console.ReadLine() ?? string.Empty;

            if (cmd.StartsWith("dbg.pg"))
            {
                var botContext = program._serviceProvider.GetService<CharacterContext>()!;
                var ability = await botContext.PurposeGenerate();
                Console.WriteLine($"[dbg] AI want to {ability.Name} ({ability.Description})");
            } else if (cmd.StartsWith("dbg.emo "))
            {
                var botContext = program._serviceProvider.GetService<CharacterContext>()!;
                botContext.Status.Emotion = cmd[8..];
            }
            else
            {
                Console.WriteLine($"Unknown command {cmd}");
            }
        }
    }
}

