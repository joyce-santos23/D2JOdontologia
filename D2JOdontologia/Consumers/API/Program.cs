using Application.Patient;
using Application.Ports;
using Consumers.API.Serialization;
using Data;
using Data.Patient;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
{
    options.Filters.Add(new InvalidJsonInputFilter()); // Adiciona o filtro global
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()); // Registra o conversor DateOnly
});

#region
builder.Services.AddScoped<IPatientManager, PatientManager>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

#endregion

#region

var connectionSring = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<ClinicaDbContext>(
    options => options.UseSqlServer(connectionSring));

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
    c.OperationFilter<AddErrorResponsesFilter>();
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
                error = error.Message // Mensagem detalhada do erro
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
