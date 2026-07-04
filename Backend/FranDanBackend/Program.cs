
using FranDanBackend;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FranDan-CPC-2026",
        Description = "Backend",
        Version = "v1.0"
    });

    // POPRAWIONY SWAGGER: Czysty opis informujący użytkownika, by wkleił TYLKO token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wprowadź sam wygenerowany token JWT (bez słowa Bearer i bez żadnych opisów).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer" // To automatycznie doda słowo "Bearer " w żądaniu!
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
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=FranDanDB.db"));

string secretKeyString = builder.Configuration["JwtSecretKey"]
    ?? throw new InvalidOperationException("JwtSecretKey is not configured.");
var secretKey = Encoding.UTF8.GetBytes(secretKeyString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddScoped<JWTGenerator>();
//Do Serwisów
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FriendService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyContext>();

    // Ta metoda sprawdza, czy tabele istnieją. Jeśli plik jest pusty, 
    // EF Core natychmiast wygeneruje w nim strukturę tabel (Users, Plans, itp.)
    context.Database.EnsureCreated();
}
if (app.Environment.IsDevelopment())
{
    //Użycie Swaggera
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //KONIECZNE DO JWT
app.UseAuthorization(); //KONIECZNE DO JWT

app.MapControllers();

app.Run();
/*
namespace FranDanBackend
{
    public class Program
    {
         public static void Main(string[] args) {
        }
    }
}
*/