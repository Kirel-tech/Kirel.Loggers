using System.Reflection;
using Kirel.Identity.Core.Models;
using Kirel.Logger.Shared.Handlers;
using Kirel.Logger.Shared.Models;
using Kirel.Logger.Messages.API.Context;
using Kirel.Logger.Messages.API.Models;
using Kirel.Logger.Messages.API.Services;
using Kirel.Repositories.Infrastructure.Generics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var authOptions = configuration.GetSection("Auth").Get<KirelAuthOptions>();
var dbConfig = configuration.GetSection("DbConfig").Get<DbConfig>();

// Add services to the container.
services.AddScoped<KirelGenericEntityFrameworkRepository<Guid, KirelLogMessage, KirelLogMessageContext>>();
services.AddScoped<KirelLogMessageService>();
services.AddApplicationContext(dbConfig);

services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = "smart";
        option.DefaultChallengeScheme = "smart";
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("APIKey", null)
    .AddPolicyScheme("smart", "Authorization Bearer JWT or user API key", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            return authHeader?.StartsWith("Bearer ") == true ? JwtBearerDefaults.AuthenticationScheme : "APIKey";
        };
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(authOptions.Key),
            ValidateIssuerSigningKey = true,
        };
    });

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kirel message logger API", Version = "v1" });

    //Set the comments path for the swagger json and ui.
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
    {  
        Name = "Authorization",  
        Type = SecuritySchemeType.ApiKey,  
        Scheme = "Bearer",  
        BearerFormat = "JWT",  
        In = ParameterLocation.Header,  
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",  
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement  
    {  
        {  
            new OpenApiSecurityScheme  
            {  
                Reference = new OpenApiReference  
                {  
                    Type = ReferenceType.SecurityScheme,  
                    Id = "Bearer"  
                }  
            },  
            new string[] {}
        }  
    });
});

//it is necessary for requests from the host locale,
//to work without a signed certificate, and we allow the use of any HTTP methods and hiders
services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", corsBuilder =>
    {
        corsBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCorsPolicy");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();