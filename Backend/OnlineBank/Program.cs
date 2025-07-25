using BussinessLayer.Interfaces;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Repositories;
using System.Text;
using BussinessLayer.Helpers;

var builder = WebApplication.CreateBuilder(args);
 
try
{
    Console.WriteLine("Starting application...");
 
    // Add database context
    builder.Services.AddDbContext<OnlineBankDbContext>(options =>
    {
        try
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Database connection string is missing"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Connection Failed: {ex.Message}");
            throw;
        }
    });



    // Configure JWT Authentication
    var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key")
        ?? throw new Exception("JWT Key is missing");
    var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer")
        ?? throw new Exception("JWT Issuer is missing");
 
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            };
 
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Token authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });
 
    // Add authorization services
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminPolicy", policy =>
            policy.RequireRole("Admin")); // Define a policy for Admin role
    });
 
    // Register services and repositories
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IDashboardService, DashboardService>();
    builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
    builder.Services.AddScoped<INetBankingService, NetBankingService>();
    builder.Services.AddScoped<INetBankingRepository, NetBankingRepository>();
    builder.Services.AddScoped<IFundTransferService, FundTransferService>();
    builder.Services.AddScoped<IFundTransferRepository, FundTransferRepository>();
    builder.Services.AddScoped<ISmtpService, SmtpService>();
    builder.Services.AddScoped<JwtHelper>(); // Add TokenService for JWT generation
 
    builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


 
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
    builder.Services.AddEndpointsApiExplorer();
 
 // Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Replace with your allowed origin
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


    var app = builder.Build();


 
    // Configure middleware
    
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAngularApp"); 
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
 
    Console.WriteLine("Application Running...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application Startup Failed: {ex.Message}");
    throw;
}