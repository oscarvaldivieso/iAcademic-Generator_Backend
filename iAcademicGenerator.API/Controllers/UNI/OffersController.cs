using iAcademicGenerator.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]

    public class OffersController : Controller
    {
        private readonly UNIServices _uniServices;

        public OffersController(UNIServices uniServices)
        {
        _uniServices = uniServices;
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _uniServices.AcademicOfferList();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        

    }
}
