using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiniWarehouse;
using MiniWarehouse.ApiClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    Timeout = TimeSpan.FromSeconds(10),
    BaseAddress = new Uri("https://localhost:7057/")
});

builder.Services.AddScoped<IApiClient, ApiClient>();

await builder.Build().RunAsync();