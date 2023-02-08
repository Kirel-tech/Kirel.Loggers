using System.Reflection;
using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kirel.Blazor.HttpLogger;
using Kirel.Blazor.HttpLogger.Listeners;
using MudBlazor.Services;

const string httpLogsStrUri = "https://localhost:7239/";
var httpLogsUri = new Uri(httpLogsStrUri);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddClipboard();

builder.Services.AddHttpClient("HttpLogs", hc => hc.BaseAddress = httpLogsUri);

builder.Services.AddScoped<NotFoundListener>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();