using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Suntech.Functions.Model;

public class Customer
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public int BirthdayInEpoch { get; init; }
    public string Email { get; init; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
