using LicenseServer.Database;
using LicenseServer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Приложение запущено.");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
