using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class RequestsController : Controller
    {
        private readonly ACAServices _ACAservices;

        public RequestsController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }

        [HttpPost("CreateAssignment")]
        public IActionResult CreateAssignment([FromBody] RequestAssignmentDTO request)
        {
            try
            {
                var result = _ACAservices.RequestAssignmentInsert(request);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Details = ex.Message
                });
            }
        }
    }
}
