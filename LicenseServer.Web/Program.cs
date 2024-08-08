using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Настройка куки
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieManager>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LicensService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<TarifService>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = TokenManager.Issuer,
		ValidAudience = TokenManager.Issuer,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenManager.Key))
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
            //context.Token = context.Request.Headers[Constans.HeaderAuthorize];
            context.Token = context.Request.Cookies[Constans.HeaderAuthorize];
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
	{
		builder.WithOrigins("https://localhost:7026") // Замените на ваш клиентский URL
			   .AllowAnyMethod()
			   .AllowAnyHeader()
			   .AllowCredentials(); // Позволяет отправку куки
	});
});
var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
	//app.UseSwagger();
	//app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Приложение запущено");



// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
