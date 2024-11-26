using Application.Consultation;
using Application.Patient;
using Application.Ports;
using Application.Schedule;
using Application.Specialist;
using Application.Specialty;
using Application.User.Ports;
using Application.User;
using Consumers.API.Serialization;
using Data;
using Data.Consultation;
using Data.Patient;
using Data.Repositories;
using Data.Schedule;
using Data.Specialist;
using Domain.Ports;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using Data.User;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Domain.Security;

var builder = WebApplication.CreateBuilder(args);

// Token Configurations
var tokenConfigurations = new TokenConfigurations();
builder.Configuration.GetSection("TokenConfigurations").Bind(tokenConfigurations);

var signingConfigurations = new SigningConfigurations();
builder.Services.AddSingleton(signingConfigurations);
builder.Services.AddSingleton(tokenConfigurations);

// Configure Authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Key is now RSA public key for validation
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingConfigurations.Key,  
        ValidateIssuer = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidateAudience = true,
        ValidAudience = tokenConfigurations.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Patient"));
    options.AddPolicy("SpecialistPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Specialist"));
});

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

#region Dependency Injection
builder.Services.AddScoped<IPatientManager, PatientManager>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IConsultationManager, ConsultationManager>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IScheduleManager, ScheduleManager>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<ISpecialistManager, SpecialistManager>();
builder.Services.AddScoped<ISpecialistRepository, SpecialistRepository>();
builder.Services.AddScoped<ISpecialtyManager, SpecialtyManager>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

#region DB Context
var connectionSring = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<ClinicaDbContext>(
    options => options.UseSqlServer(connectionSring));
#endregion

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ScheduleDtoExample>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "D2J API",
        Version = "v1",
        Description = "API for managing dental appointments",
        TermsOfService = new Uri("https://github.com/joyce-santos23"),
        Contact = new OpenApiContact
        {
            Name = "Joyce Santos Mendes",
            Email = "joycectba@hotmail.com",
            Url = new Uri("https://github.com/joyce-santos23")
        },
        License = new OpenApiLicense
        {
            Name = "License Terms",
            Url = new Uri("https://github.com/joyce-santos23")
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' followed by a space and your JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    c.EnableAnnotations();
    c.ExampleFilters();
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 400; // Bad Request
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        if (error != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                error = error.Message
            });
        }
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Add Authentication Middleware
app.UseAuthorization();  // Add Authorization Middleware

app.MapControllers();

app.Run();
