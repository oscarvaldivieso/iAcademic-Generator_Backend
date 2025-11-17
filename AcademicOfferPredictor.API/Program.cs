using AcademicOfferPredictor; // OfferPredictor del proyecto ML
using AcademicOfferPredictor.API.Services.Implementation;
using AcademicOfferPredictor.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ====== Services ======
builder.Services.AddControllers();

// CORS simple para pruebas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Connection string desde appsettings.json
var connString = builder.Configuration.GetConnectionString("AcademicDb")!;

// OfferPredictor de ML como singleton (usa tu clase grande)
builder.Services.AddSingleton(sp =>
    new OfferPredictor(
        connectionString: connString,
        sourceView: "STG.vw_ofertas_enriched",
        openThreshold: 12f,
        sigma: 5f,
        numberOfLeaves: 64,
        minExampleCount: 10,
        numberOfBits: 15,
        bulkBatchSize: 1000,
        resultTable: "ML.pred_ofertas_resultados",
        saveModel: true
    )
);

// Servicio de API que envuelve al predictor
builder.Services.AddScoped<IOfferPredictorService, OfferPredictorService>();

var app = builder.Build();

// ====== Middleware ======
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
