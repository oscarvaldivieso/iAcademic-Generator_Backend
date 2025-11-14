using AcademicOfferPredictor.API.Services.Interfaces;
using AcademicOfferPredictor.API.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// 1) Servicios
builder.Services.AddControllers();

// OpenAPI integrado en .NET (sin Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Servicio dummy del ML
builder.Services.AddScoped<IOfferPredictorService, OfferPredictorService>();

var app = builder.Build();

// 2) Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Expone el documento OpenAPI en /openapi/v1.json
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3) Controllers
app.MapControllers();

app.Run();
