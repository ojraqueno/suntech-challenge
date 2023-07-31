namespace Suntech.Functions.Data;

public class SuntechCosmosDbOptions
{
    public string Endpoint { get; set; }
    public string Key { get; set; }
    public string Database { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(Endpoint) && !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Database);
}
