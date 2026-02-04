using CdsBackend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5088")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseCors("AllowFrontend");

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/ping", () => "pong");

app.MapGet("/api/cds/steps", () =>
{
    var steps = new List<CdsStep>
    {
        new CdsStep { Id = 1, Title = "Patient Details", Description = "Collect basic patient information" },
        new CdsStep { Id = 2, Title = "Symptoms", Description = "Record presenting symptoms" },
        new CdsStep { Id = 3, Title = "History", Description = "Capture relevant medical history" },
        new CdsStep { Id = 4, Title = "Assessment", Description = "Clinical assessment and observations" },
        new CdsStep { Id = 5, Title = "Plan", Description = "Recommended actions and follow-up" }
    };

    return Results.Ok(steps);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
