using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClinicFrontend;
using ClinicFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point HttpClient to backend API (ClinicService)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:7235/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<ClinicApi>();

await builder.Build().RunAsync();
