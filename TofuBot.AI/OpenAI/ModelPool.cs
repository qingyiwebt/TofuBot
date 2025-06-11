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
}