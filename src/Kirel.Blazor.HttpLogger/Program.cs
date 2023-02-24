using System.Reflection;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kirel.Blazor.HttpLogger;
using Kirel.Blazor.HttpLogger.Listeners;
using Kirel.Identity.Client.Blazor.Services;
using Kirel.Identity.Client.Interfaces;
using Kirel.Identity.Client.Jwt.Handlers;
using Kirel.Identity.Client.Jwt.Options;
using Kirel.Identity.Client.Jwt.Providers;
using Kirel.Identity.Client.Jwt.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var defaultBaseUri = new Uri(builder.HostEnvironment.BaseAddress);
var hostOnlyBaseStr =  $"{defaultBaseUri.Scheme}://{defaultBaseUri.Host}:{defaultBaseUri.Port}/";

var identityUriStr = builder.Configuration["Services:Identity"];
var httpLogsStrUri = builder.Configuration["Services:HttpLogsApi"];
if (string.IsNullOrWhiteSpace(httpLogsStrUri))
{
    Console.WriteLine("Http logs api address is not set in appsettings.json");
    httpLogsStrUri = hostOnlyBaseStr;
}
if (string.IsNullOrWhiteSpace(identityUriStr))
{
    Console.WriteLine("Identity address is not set in appsettings.json");
    identityUriStr = hostOnlyBaseStr;
}

var httpLogsUri = new Uri(httpLogsStrUri);

builder.Services.AddMudServices();

builder.Services.AddClipboard();

// Add http client JWT Authorization handler to DI
builder.Services.AddScoped<KirelJwtHttpClientAuthorizationHandler>();

builder.Services.AddHttpClient("HttpLogs", hc => hc.BaseAddress = httpLogsUri)
    .AddHttpMessageHandler<KirelJwtHttpClientAuthorizationHandler>();

builder.Services.AddScoped<NotFoundListener>();

// Add client token storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IClientTokenService, KirelBlazoredClientTokenService>();

// Add http client factory instance with default http client for fetch data example
builder.Services.AddHttpClient(string.Empty, hc => hc.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Add JWT authentication service with configuration for working with Example project API endpoint
builder.Services.AddScoped<IClientAuthenticationService, KirelClientJwtAuthenticationService>();
builder.Services.Configure<KirelClientJwtAuthenticationOptions>(options =>
{
    options.BaseUrl = identityUriStr;
    options.RelativeUrl = "authentication/jwt"; // From Example project /Controllers/ExJwtAuthenticationController Route Attribute
});

// Add JWT Authentication state provider
builder.Services.AddScoped<KirelJwtTokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<KirelJwtTokenAuthenticationStateProvider>());

// Add AutoMapper dto <--> dto mappings
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();