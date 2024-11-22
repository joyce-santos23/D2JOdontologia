using Application.Consultation;
using Application.Patient;
using Application.Ports;
using Application.Schedule;
using Application.Specialist;
using Application.Specialty;
using Consumers.API.Serialization;
using Data;
using Data.Consultation;
using Data.Patient;
using Data.Repositories;
using Data.Schedule;
using Data.Specialist;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()); // Registra o conversor DateOnly
});

#region
builder.Services.AddScoped<IPatientManager, PatientManager>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

#endregion
builder.Services.AddScoped<IConsultationManager, ConsultationManager>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IPatientManager, PatientManager>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IScheduleManager, ScheduleManager>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<ISpecialistManager, SpecialistManager>();
builder.Services.AddScoped<ISpecialistRepository, SpecialistRepository>();
builder.Services.AddScoped<ISpecialtyManager, SpecialtyManager>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

#region

var connectionSring = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<ClinicaDbContext>(
    options => options.UseSqlServer(connectionSring));

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExamplesFromAssemblyOf<ScheduleDtoExample>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "D2J API",
        Version = "v1",
        Description = "API for managing hotel bookings, guests, and rooms",
        TermsOfService = new Uri("https://github.com/joyce-santos23"),
        Contact = new OpenApiContact
        {
            Name = "Joyce Santos Mendes",
            Email = "joycectba@hotmail.com",
            Url = new Uri("https://github.com/joyce-santos23")
        },
        License = new OpenApiLicense
        {
            Name = "Termo de Licença de Uso",
            Url = new Uri("https://github.com/joyce-santos23")
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

app.UseAuthorization();

app.MapControllers();

app.Run();
