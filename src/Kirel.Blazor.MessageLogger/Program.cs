using System.Reflection;
using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kirel.Blazor.MessageLogger;
using MudBlazor.Services;

const string logsMessageStrUri = "https://localhost:7229/";
var logsMessageUri = new Uri(logsMessageStrUri);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddClipboard();

builder.Services.AddHttpClient("MessageLogs", hc => hc.BaseAddress = logsMessageUri);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();