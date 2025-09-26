using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class AreasController : Controller
    {
        private readonly ACAServices _ACAservices;

        public AreasController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }



        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _ACAservices.ListAreas();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AreasDTO areas)
        {

            try
            {
                var result = _ACAservices.AreasInsert(areas);

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

        [HttpPost("Update")]
        public IActionResult Update([FromBody] AreasDTO areas)
        {

            try
            {
                var result = _ACAservices.AreasUpdate(areas);

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

        [HttpDelete("Delete")]
        public IActionResult Delete(string areCode)
        {
            var result = _ACAservices.AreasDelete(areCode);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }







    }
}
