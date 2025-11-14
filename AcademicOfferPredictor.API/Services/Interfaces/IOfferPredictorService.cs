using System.Threading;
using System.Threading.Tasks;
using AcademicOfferPredictor.API.Models;

namespace AcademicOfferPredictor.API.Services.Interfaces
{
    public interface IOfferPredictorService
    {
        Task<PredictionRunResult> RunAsync(string period, CancellationToken ct = default);
    }
}
