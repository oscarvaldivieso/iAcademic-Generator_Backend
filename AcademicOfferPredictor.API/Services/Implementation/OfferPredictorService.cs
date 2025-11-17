using System;
using System.Threading;
using System.Threading.Tasks;
using AcademicOfferPredictor;                      // Proyecto ML (OfferPredictor)
using AcademicOfferPredictor.API.Models;
using AcademicOfferPredictor.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AcademicOfferPredictor.API.Services.Implementation
{
    public class OfferPredictorService : IOfferPredictorService
    {
        private readonly OfferPredictor _predictor;
        private readonly ILogger<OfferPredictorService> _logger;

        public OfferPredictorService(
            OfferPredictor predictor,
            ILogger<OfferPredictorService> logger)
        {
            _predictor = predictor;
            _logger = logger;
        }

        public async Task<PredictionRunResult> RunAsync(string period, CancellationToken ct = default)
        {
            _logger.LogInformation("[ML] Ejecutando predictor real para periodo {Period}...", period);

            // Ahora tu OfferPredictor ya recibe el periodo:
            await _predictor.RunAutoAsync(period);

            _logger.LogInformation("[ML] Ejecución completada para periodo {Period}.", period);

            var now = DateTime.Now;
            var runTag = $"{period}-API-{now:yyyyMMdd-HHmmss}";

            // De momento dejamos métricas dummy
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
