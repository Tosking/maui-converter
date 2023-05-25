using Android.Util;

namespace Converter;
using Newtonsoft.Json;

public class Model
{
    private HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://www.cbr-xml-daily.ru"),
    };
    public async Task<Data> GetAsync(DateTime date)
    {
        Data list = null;
        var bufdate = $"{date:yyyy/MM/dd}".Replace(".", "//");
        HttpResponseMessage response;
        if (DateTime.Now == date)
        {
            response = 
                await httpClient.GetAsync($"https://www.cbr-xml-daily.ru/daily_json.js");
        }
        else
        {
            response =
                await httpClient.GetAsync($"https://www.cbr-xml-daily.ru/archive/{bufdate}/daily_json.js");
        }

        dynamic json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
        if (json?["code"] == 404)
        {
            return await GetAsync(date.AddDays(-1));
        }
        Data jsonResponse = JsonConvert.DeserializeObject<Data>(await response.Content.ReadAsStringAsync());
        if (jsonResponse != null)
        {
            jsonResponse.Valute.Add("RUB", new Currency
            {
                CharCode = "RUB",
                ID = "0",
                Name = "Российский рубль",
                Nominal = 1,
                Value = 1.0,
            });
        }
        return jsonResponse;
    }
}