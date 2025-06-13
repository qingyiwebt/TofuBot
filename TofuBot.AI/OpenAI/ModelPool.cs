using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

namespace TofuBot.AI.OpenAI;

public class ModelPool
{
    private List<ModelOptions> _modelOptions = [];
    public void AddModel(ModelOptions modelOptions)
    {
        _modelOptions.Add(modelOptions);
    }

    public void RemoveModel(string usage)
    {
        _modelOptions.RemoveAll(x => x.Usage == usage);
    }

    public ModelOptions GetModelOptions(string usage)
    {
        return _modelOptions.First(x => x.Usage == usage);
    }

    public ChatClient NewChatClient(string usage)
    {
        var options = _modelOptions.First(x => x.Usage == usage);
        var chatClient = new ChatClient(
            model: options.ModelName,
            credential: new ApiKeyCredential(options.SecretKey),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint)
            });
        
        return chatClient;
    }
}