namespace Suntech.Functions.ServiceBus;

public class SuntechServiceBusClientOptions
{
    public string ConnectionString { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(ConnectionString);
}
