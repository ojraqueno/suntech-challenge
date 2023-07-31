namespace Suntech.Functions.EventGrid;

public class SuntechEventGridOptions
{
    public string Endpoint { get; set; }
    public string Key { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(Endpoint) && !string.IsNullOrWhiteSpace(Key);
}
