using System.Text;
using Microsoft.AspNetCore.Builder;           // ayuda a IntelliSense
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using UploadData.API.Services;                // 👈 debe coincidir con los namespaces de abajo

// Necesario para ExcelDataReader (code pages en Linux/Docker)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// MVC + endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + IFormFile como "binary"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UploadData API",
        Version = "v1",
        Description = "Endpoints para cargar Excel de oferta académica"
    });
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

// DI del servicio
builder.Services.AddScoped<IUploadService, UploadService>();

// CORS (dev)
builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Límites de carga (~200MB)
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 200_000_000;
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
});
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 200_000_000);

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
