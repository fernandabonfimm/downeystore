using RestaurantQueue.Services;
using RestaurantQueue.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStorage, InMemoryStorage>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPreparationService, PreparationService>();

var app = builder.Build();

// Swagger sempre disponível
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantQueue API v1");
    options.RoutePrefix = string.Empty; // Swagger na raiz
});

app.MapControllers();

app.MapGet("/hello", () => "Hello World!").WithTags("Hello");

app.Run();
