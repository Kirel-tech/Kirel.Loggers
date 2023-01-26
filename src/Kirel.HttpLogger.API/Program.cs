using System.Reflection;
using Kirel.HttpLogger.API.Context;
using Kirel.HttpLogger.API.Models;
using Kirel.HttpLogger.API.Services;
using Kirel.Logger.Shared.Models;
using Kirel.Repositories.Infrastructure.Generics;
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
services.AddScoped<KirelGenericEntityFrameworkRepository<Guid, KirelLogHttp, KirelLogHttpDbContext>>();
services.AddScoped<KirelLogHttpService>();
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