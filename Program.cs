var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure the cors policy to allow requests from any origin
var frontendOrigin = builder.Configuration["FrontendOrigin"];
Console.WriteLine($"Loaded FrontendOrigin: {frontendOrigin}");

if (string.IsNullOrEmpty(frontendOrigin))
{
    throw new ArgumentNullException(nameof(frontendOrigin), "FrontendOrigin is not configured in appsettings.json.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(frontendOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// use cors middleware
app.UseCors("FrontendPolicy");


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Add Swagger UI for interacting with the API
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
