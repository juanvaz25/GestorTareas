using GestorTareas.Repositories;
using GestorTareas.Data;
using GestorTareas.Middleware;
using GestorTareas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ── Base de datos ────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()
    )
);

// ── Inyección de dependencias ────────────────────────────────────────────────
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// ── Controladores ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── CORS (permite que el frontend React consuma la API) ──────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(
                    builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? new[] { "http://localhost:5173" }
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

// ── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestor de Tareas API",
        Version = "v1",
        Description = "API REST para gestión de tareas — STJ La Pampa",
        Contact = new OpenApiContact { Name = "Secretaría de Sistemas y Organización" }
    });
    c.AddServer(new OpenApiServer
    {
        Url = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5135",
        Description = "URL base para la API"
    });
    // Incluir comentarios XML en Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// ── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestor de Tareas API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Necesario para que xUnit pueda acceder al WebApplicationFactory
public partial class Program { }