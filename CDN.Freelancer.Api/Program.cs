using CDN.Freelancer.Infrastructure;
using CDN.Freelancer.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using CDN.Freelancer.Api.Middleware;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(options =>
{
    // Disable automatic model validation for navigation properties
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.AddJsonOptions(options =>
{
    // Handle circular references in JSON serialization
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
})
.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateFreelancerDtoValidator>();
    fv.DisableDataAnnotationsValidation = true;
});

// Register validators explicitly
builder.Services.AddScoped<IValidator<CreateFreelancerDto>, CreateFreelancerDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateFreelancerDto>, UpdateFreelancerDtoValidator>();
builder.Services.AddScoped<IValidator<SkillsetDto>, SkillsetDtoValidator>();
builder.Services.AddScoped<IValidator<HobbyDto>, HobbyDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure model validation to ignore navigation properties
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

// Configure EF Core and infrastructure
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=freelancer.db");

// Register application service
builder.Services.AddScoped<FreelancerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS middleware
app.UseCors("AllowAll");

// Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
