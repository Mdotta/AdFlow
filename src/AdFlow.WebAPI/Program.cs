using AdFlow.Application.Interfaces;
using AdFlow.Application.Services;
using AdFlow.Domain.Interfaces;
using AdFlow.Infrastructure.Data;
using AdFlow.Infrastructure.Repositories;
using AdFlow.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configure Database
builder.Services.AddDbContext<AdFlowDbContext>(options =>
    options.UseInMemoryDatabase("AdFlowDb"));

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "AdFlow-SuperSecret-Key-For-JWT-Token-Generation-2024";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AdFlow";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AdFlowUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Register services
builder.Services.AddScoped<IFacebookUserRepository, FacebookUserRepository>();
builder.Services.AddScoped<IFacebookAuthService, FacebookAuthService>();
builder.Services.AddScoped<IJwtTokenService>(provider => 
    new JwtTokenService(jwtKey, jwtIssuer, jwtAudience));
builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
