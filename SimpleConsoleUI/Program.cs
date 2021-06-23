using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleConsoleUI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiKey = "6cb804580cbb60e243edce26b0d516bf";
            string city = "Center Moriches";
            string state = "New York";

            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={city},{state}&appid={apiKey}&units=imperial"),
            };

            using (HttpClient client = new())
            {
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        object body = await response.Content.ReadFromJsonAsync<object>();
                        Console.WriteLine(JsonSerializer.Serialize<object>(body, new JsonSerializerOptions { WriteIndented = true }));
                    }
                    else
                    {
                        Console.WriteLine($"Request failed[{ response.StatusCode }]: { response.ReasonPhrase }");
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
