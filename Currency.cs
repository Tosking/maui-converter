namespace Converter;
using Newtonsoft.Json;

public class Currency
{
    [JsonProperty("ID")]
    public string ID { get; set; }

    [JsonProperty("NumCode")]
    public string NumCode { get; set; }

    [JsonProperty("CharCode")]
    public string CharCode { get; set; }

    [JsonProperty("Nominal")]
    public int Nominal { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Value")]
    public double Value { get; set; }

    [JsonProperty("Previous")]
    public double Previous { get; set; }

    public override string ToString()
    {
        return Name; 
    }
}