using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiniWarehouse;
using MiniWarehouse.ApiClient;
using MiniWarehouse.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7057/")
});

builder.Services.AddScoped<IApiClient, ApiClient>();

builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<CategoryService>();

await builder.Build().RunAsync();