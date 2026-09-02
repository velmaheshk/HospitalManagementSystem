using HospitalManagement.API.Middleware;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Mappings;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Interface;
using HospitalManagement.Domain.Interface.IRepository;
using HospitalManagement.Domain.IRepository;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using HospitalManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
//using HospitalManagement.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddControllers();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(AppointmentProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital Management API",
        Version = "v1",
        Description = "ASP.NET Core Web API for Hospital Management System"
    });
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IBillRepository, BillRepo>();

builder.Services.AddScoped<BillService, BillService>();
builder.Services.AddScoped<IBillRepository, BillService>();
builder.Services.AddControllers();



builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IPasswordHasher,PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();

// Application interfaces -> Infrastructure implementations
builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<ApplicationDbContext>());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ========================================================
// JWT
// ========================================================

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is missing.");

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    builder.Configuration["Jwt:Audience"];


builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtKey)),

                ClockSkew =
                    TimeSpan.Zero
            };
    });



// Authorization
//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    // or policy.RequireClaim("role", "Admin");
    // or a custom requirement
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

//app.UseHttpsRedirection();
 

app.MapControllers();

app.Run();