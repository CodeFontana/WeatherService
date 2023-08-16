using Polly.Contrib.WaitAndRetry;
using Polly;
using WeatherServiceApp.Interfaces;
using WeatherServiceApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ICurrentWeatherService, CurrentWeatherService>();
builder.Services.AddHttpClient("currentWeather", client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
}).AddTransientHttpErrorPolicy(builder =>
    builder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
        TimeSpan.FromSeconds(1), 5)));
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
