using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]
    public class CampusController : Controller
    {
        private readonly UNIServices _UNIservices;

        public CampusController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }

        
        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _UNIservices.ListCampus();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CampusDTO campus)
        {

            try
            {
                var result = _UNIservices.CampusInsert(campus);

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
        
        [HttpPost("update")]
        public IActionResult Update([FromBody] CampusDTO campus)
        {

            try
            {
                var result = _UNIservices.CampusUpdate(campus);

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
        
        [HttpDelete("delete")]
        public IActionResult Delete(string campusCodigo)
        {
            var result = _UNIservices.CampusDelete(campusCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
