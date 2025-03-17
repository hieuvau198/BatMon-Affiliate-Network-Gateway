using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS (Important for Azure)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

// Enable Swagger **AFTER** Ocelot
app.UseSwagger();
app.UseSwaggerUI();

// Apply CORS
app.UseCors("AllowAll");

// Use Ocelot Middleware (should be before `UseAuthorization`)
app.UseOcelot().Wait();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
