using Newtonsoft.Json;
namespace Converter;

public class Data
{
    [JsonProperty("Date")]
    public DateTime Date { get; set; }

    [JsonProperty("PreviousDate")]
    public DateTime PreviousDate { get; set; }

    [JsonProperty("PreviousURL")]
    public string PreviousURL { get; set; }

    [JsonProperty("Timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("Valute")]
    public Dictionary<string, Currency> Valute { get; set; }
}