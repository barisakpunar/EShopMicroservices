using Catalog.API.Data;
using HealthChecks.UI.Client;
using JasperFx;

var builder = WebApplication.CreateBuilder(args);
var  assembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(assembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database"));

}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => {});

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
