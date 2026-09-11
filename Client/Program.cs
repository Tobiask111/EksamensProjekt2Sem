using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Client.Service;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{Server.Url}") });
// Dette får appen til at bruge den samme adresse, som den selv ligger på
/* builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }); */

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<ProjectService>();


await builder.Build().RunAsync();