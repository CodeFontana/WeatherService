using Polly.Contrib.WaitAndRetry;
using Polly;
using WeatherServiceApp.Services;
using WeatherServiceApp.Features;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    });
builder.Services.AddJSComponents();
builder.Services.AddResponseCompression();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ICurrentWeatherService, CurrentWeatherService>();
builder.Services.AddHttpClient("currentWeather", client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
}).AddTransientHttpErrorPolicy(builder =>
    builder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
        TimeSpan.FromSeconds(1), 5)));
WebApplication app = builder.Build();

app.UseExceptionHandler("/Error", createScopeForErrors: true);

if (app.Environment.IsDevelopment() == false)
{
    app.UseHsts();
    app.UseResponseCompression();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();
app.Run();
