using api_with_auth;
using api_with_auth.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(/*x=>x.Filters.Add<ApiKeyAuthFilter>()*/);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The Api Key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement {
        {scheme, new List<string>() }
    };

    x.AddSecurityRequirement(requirement);
});

builder.Services.AddScoped<ApiKeyAuthFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();

var group = app.MapGroup("api2").AddEndpointFilter<ApiKeyEndpointFilter>();
group.MapGet("/weather", () =>
{
    var rng = new Random();
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Globals.Summaries[rng.Next(Globals.Summaries.Length)]
    }).ToArray();
});//.AddEndpointFilter<ApiKeyEndpointFilter>();

app.MapGet("/", () => "Hello! This is example to dotnet api with api key!");
app.MapControllers();

app.Run();
