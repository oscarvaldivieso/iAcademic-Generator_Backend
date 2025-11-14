using Microsoft.AspNetCore.Mvc;
using AcademicOfferPredictor.API.Services.Interfaces;
using AcademicOfferPredictor.API.Models;

namespace AcademicOfferPredictor.API.Controllers
{
    [ApiController]
    [Route("api/ml")]
    public class MlController : ControllerBase
    {
        private readonly IOfferPredictorService _service;

        public MlController(IOfferPredictorService service)
        {
            _service = service;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Run([FromBody] MlRunRequest request)
        {
            var result = await _service.RunAsync(request.Period);
            return Ok(result);
        }
    }

    public class MlRunRequest
    {
        public string Period { get; set; } = "";
    }
}
