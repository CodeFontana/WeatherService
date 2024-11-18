using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

string apiKey = "<API key from openweathermap.org>";
string city = "Center Moriches";
string state = "New York";

HttpRequestMessage request = new()
{
    Method = HttpMethod.Get,
    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={city},{state}&appid={apiKey}&units=imperial"),
};

using HttpClient client = new();
using HttpResponseMessage response = await client.SendAsync(request);

if (response.IsSuccessStatusCode)
{
    object? result = await response.Content.ReadFromJsonAsync<object>();

    if (result is null)
    {
        Console.WriteLine($"Failed to retrieve weather data for City={city} and State={state}");
        return;
    }

    JsonSerializerOptions options = new() { WriteIndented = true };
    Console.WriteLine(JsonSerializer.Serialize(result, options));
}
else
{
    Console.WriteLine($"Request failed[{response.StatusCode}]: {response.ReasonPhrase}");
}
