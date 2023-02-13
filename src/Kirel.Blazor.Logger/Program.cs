using System.Reflection;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.Clipboard;
using Kirel.Blazor.HttpLogger.Listeners;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kirel.Blazor.Logger;
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

var identityUriStr = builder.Configuration["Services:Identity"];
var httpLogsStrUri = builder.Configuration["Services:HttpLogsApi"];
var logsMessageStrUri = builder.Configuration["Services:MessageLogsApi"];

var httpLogsUri = new Uri(httpLogsStrUri);
var logsMessageUri = new Uri(logsMessageStrUri);

builder.Services.AddMudServices();

// Add client token storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IClientTokenService, KirelBlazoredClientTokenService>();

builder.Services.AddClipboard();

builder.Services.AddScoped<NotFoundListener>();
// Add http client JWT Authorization handler to DI
builder.Services.AddScoped<KirelJwtHttpClientAuthorizationHandler>();

builder.Services.AddHttpClient("HttpLogs", hc => hc.BaseAddress = httpLogsUri)
    .AddHttpMessageHandler<KirelJwtHttpClientAuthorizationHandler>();
builder.Services.AddHttpClient("MessageLogs", hc => hc.BaseAddress = logsMessageUri)
    .AddHttpMessageHandler<KirelJwtHttpClientAuthorizationHandler>();

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