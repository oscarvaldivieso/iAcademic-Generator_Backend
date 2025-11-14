using System;
using System.Threading;
using System.Threading.Tasks;
using AcademicOfferPredictor;                      // <- Proyecto ML
using AcademicOfferPredictor.API.Models;
using AcademicOfferPredictor.API.Services.Interfaces;

namespace AcademicOfferPredictor.API.Services.Implementation
{
    public class OfferPredictorService : IOfferPredictorService
    {
        public async Task<PredictionRunResult> RunAsync(string period, CancellationToken ct = default)
        {
            const string connectionString =
                "Server=iAcademicGenerator.mssql.somee.com;Database=iAcademicGenerator;User Id=oscarvaldivieso_SQLLogin_1;Password=admin123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

            const string sourceView = "STG.vw_ofertas_enriched";
            const float openThreshold = 12f;
            const float sigma = 5f;
            const int numberOfLeaves = 64;
            const int minExampleCount = 10;
            const int numberOfBits = 15;
            const int bulkBatchSize = 1000;
            const string resultTable = "ML.pred_ofertas_resultados";
            const bool saveModel = true;

            // Instanciamos tu motor real de ML.NET
            var predictor = new OfferPredictor(
                connectionString,
                sourceView,
                openThreshold,
                sigma,
                numberOfLeaves,
                minExampleCount,
                numberOfBits,
                bulkBatchSize,
                resultTable,
                saveModel);

            // OJO: por ahora RunAutoAsync sigue usando internamente "20254".
            // Más adelante lo modificamos para aceptar el periodo como parámetro.
            Console.WriteLine($"[ML] Ejecutando predictor real para periodo {period}...");
            await predictor.RunAutoAsync();
            Console.WriteLine("[ML] Ejecución completada.");

            var now = DateTime.Now;
            var runTag = $"{period}-API-{now:yyyyMMdd-HHmmss}";

            // De momento devolvemos métricas dummy, luego las sacamos del modelo si las exponemos.
            var metrics = new
            {
                RMSE = 0.0,
                RSquared = 0.0
            };

            return new PredictionRunResult
            {
                Period = period,
                RunTag = runTag,
                Status = "Succeeded",
                Metrics = metrics
            };
        }
    }
}
