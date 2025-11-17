namespace AcademicOfferPredictor.API.Models
{
    public class PredictionRunResult
    {
        public string Period { get; set; } = "";
        public string RunTag { get; set; } = "";
        public string Status { get; set; } = "OK";
        public object? Metrics { get; set; }
    }
}
