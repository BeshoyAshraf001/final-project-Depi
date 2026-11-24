using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileAdvisor.BLL.Services.Implementations;
using MobileAdvisor.BLL.Services.Interfaces;
using MobileAdvisor.DAL.Data;
using MobileAdvisor.DAL.Repositories.Implementations;
using MobileAdvisor.DAL.Repositories.Interfaces;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
    

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Add UnitOfWork & Repositories (DAL)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IExpertProfileRepository, ExpertProfileRepository>();

// 3. Add Business Logic Services (BLL)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExpertService, ExpertService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IPhoneService, PhoneService>();

// 4. JWT Authentication configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MobileAdvisorSecretKeyLongerThan32BytesForHmacSha256SecurityTokenAuth!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MobileAdvisorAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MobileAdvisorClients";

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

// 5. Add Controllers & CORS
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 6. Built-in OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interactive Modern API Reference UI
}
// app.Map("/",()=>Redirect("/scalar"));
app.Map("/", () => Results.Redirect("/scalar"));
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
