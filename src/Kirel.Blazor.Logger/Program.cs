using System.Reflection;
using CurrieTechnologies.Razor.Clipboard;
using Kirel.Blazor.HttpLogger.Listeners;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kirel.Blazor.Logger;
using MudBlazor.Services;

const string httpLogsStrUri = "https://localhost:7239/";
const string logsMessageStrUri = "https://localhost:7229/";

var httpLogsUri = new Uri(httpLogsStrUri);
var logsMessageUri = new Uri(logsMessageStrUri);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddClipboard();

builder.Services.AddScoped<NotFoundListener>();

builder.Services.AddHttpClient("HttpLogs", hc => hc.BaseAddress = httpLogsUri);
builder.Services.AddHttpClient("MessageLogs", hc => hc.BaseAddress = logsMessageUri);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();