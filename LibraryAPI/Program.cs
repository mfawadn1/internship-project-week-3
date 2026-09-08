using System.Text;
using LibraryAPI.Data;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Bearer authorization support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "ASP.NET Core Web API with JWT Authentication and EF Core"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token format: Bearer {your token}"
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
            Array.Empty<string>()
        }
    });
});

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretLibraryKeyForWeek4InternshipJWTTokenValidation2026!";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// 2. Configure Entity Framework Core with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// 3. Register Dependency Injection Services
// Check database connectivity at startup; use InMemory fallback if SQL Server is not locally active
bool canConnectDb = false;
try
{
    var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
    optionsBuilder.UseSqlServer(connectionString);
    using var tempContext = new LibraryDbContext(optionsBuilder.Options);
    canConnectDb = tempContext.Database.CanConnect();
    if (canConnectDb)
    {
        tempContext.Database.EnsureCreated();
    }
}
catch
{
    canConnectDb = false;
}

if (canConnectDb)
{
    Console.WriteLine("[Database]: Connected to SQL Server (LibraryDb_Week3). Using EF Core BookRepository.");
    builder.Services.AddScoped<IBookRepository, BookRepository>();
}
else
{
    Console.WriteLine("[Database]: SQL Server not detected or unreachable. Seamlessly using InMemoryBookRepository for live development.");
    builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
}

builder.Services.AddScoped<IBookService, BookService>();

// 4. Configure CORS for Angular Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseHttpsRedirection();

// IMPORTANT: Authentication must precede Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
