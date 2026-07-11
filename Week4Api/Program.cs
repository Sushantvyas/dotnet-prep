using Week4Api;
using Week4Api.Filters;
using Week4Api.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddTransient<ITransientCounter, Counter>();
builder.Services.AddScoped<IScopedCounter, Counter>();
builder.Services.AddSingleton<ISingletonCounter, Counter>();
//builder.Services.AddSingleton<IBadSingletonService, BadSingletonService>();
builder.Services.AddSingleton<IBadSingletonService, FixedSingletonService>();
builder.Services.AddTransient<INotifier, EmailNotifier>();
builder.Services.AddTransient<INotifier, SlackNotifier>();
builder.Services.AddTransient<INotifier, SmsNotifier>();
//builder.Services.Configure<EmailSettings>(
//    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddOptions<EmailSettings>()
    .BindConfiguration("EmailSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<LogActionFilter>();
});

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"[MW1] Before - {context.Request.Path}");
//    await next(context);
//    Console.WriteLine($"[MW1] After - {context.Response.StatusCode}");
//});

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"[MW2] Before - {context.Request.Path}");
//    await next(context);
//    Console.WriteLine($"[MW2] After - {context.Response.StatusCode}");
//});

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"[MW3] Before - {context.Request.Path}");
//    await next(context);
//    Console.WriteLine($"[MW3] After: ");
//});

//app.UseMiddleware<RequestLoggingMiddleware>();
//app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();
// Configure the HTTP request pipeline.

var products = app.MapGroup("/api/products");

var productStore = new List<Product>
{
    new(1, "Laptop", 999.99m),
    new(2, "Mouse", 29.99m),
    new(3, "keyboard", 79.99m)
};

products.MapGet("/", () => Results.Ok(productStore));

products.MapGet("/{id:int}", (int id) =>
{
    var product = productStore.FirstOrDefault(u => u.Id == id);
    return product is null ? Results.NotFound() : Results.Ok(product);
}
);

products.MapPost("/", (Product product) =>
{
    product = product with { Id = productStore.Count + 1 };
    productStore.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
});

products.MapDelete("/{id:int}", (int id)
    =>
{
    var product = productStore.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound();
    productStore.Remove(product);
    return Results.NoContent();
});


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record Product(int Id, string Name, decimal Price);