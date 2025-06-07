var builder = WebApplication.CreateBuilder(args);

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

// In-memory store
var forecasts = new List<WeatherForecast>();
var idCounter = 1;

// CREATE
app.MapPost("/weatherforecast", (WeatherForecastInput input) =>
{
    var forecast = new WeatherForecast
    {
        Id = idCounter++,
        Date = input.Date,
        TemperatureC = input.TemperatureC,
        Summary = input.Summary
    };
    forecasts.Add(forecast);
    return Results.Created($"/weatherforecast/{forecast.Id}", forecast);
});

// READ ALL
app.MapGet("/weatherforecast", () =>
{
    return Results.Ok(forecasts);
});

// READ BY ID
app.MapGet("/weatherforecast/{id:int}", (int id) =>
{
    var forecast = forecasts.FirstOrDefault(f => f.Id == id);
    return forecast is not null ? Results.Ok(forecast) : Results.NotFound();
});

// UPDATE
app.MapPut("/weatherforecast/{id:int}", (int id, WeatherForecastInput input) =>
{
    var forecast = forecasts.FirstOrDefault(f => f.Id == id);
    if (forecast is null) return Results.NotFound();

    forecast.Date = input.Date;
    forecast.TemperatureC = input.TemperatureC;
    forecast.Summary = input.Summary;

    return Results.Ok(forecast);
});

// DELETE
app.MapDelete("/weatherforecast/{id:int}", (int id) =>
{
    var forecast = forecasts.FirstOrDefault(f => f.Id == id);
    if (forecast is null) return Results.NotFound();

    forecasts.Remove(forecast);
    return Results.NoContent();
});

app.Run();

// Models
record WeatherForecastInput(DateOnly Date, int TemperatureC, string? Summary);

class WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}