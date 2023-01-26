using System.Reflection;
using Kirel.Logger.Shared.Models;
using Kirel.MessageLogger.API.Context;
using Kirel.MessageLogger.API.Models;
using Kirel.MessageLogger.API.Services;
using Kirel.Repositories.Infrastructure.Generics;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var ymlDeserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

var ymlString = File.ReadAllText("DbConfig.yaml");
var dbConfig = ymlDeserializer.Deserialize<DbConfig>(ymlString);

// Add services to the container.
services.AddScoped<KirelGenericEntityFrameworkRepository<Guid, KirelLogMessage, KirelLogMessageContext>>();
services.AddScoped<KirelLogMessageService>();
services.AddApplicationContext(dbConfig);

services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();