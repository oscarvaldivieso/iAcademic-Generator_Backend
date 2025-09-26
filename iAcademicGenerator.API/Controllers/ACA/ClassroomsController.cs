using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class ClassroomsController : Controller
    {
        private readonly ACAServices _ACAservices;

        public ClassroomsController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }



        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _ACAservices.ListClassrooms();

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
        public IActionResult Create([FromBody] ClassroomsDTO classrooms)
        {

            try
            {
                var result = _ACAservices.ClassroomsInsert(classrooms);

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
        public IActionResult Update([FromBody] ClassroomsDTO classrooms)
        {

            try
            {
                var result = _ACAservices.ClassroomsUpdate(classrooms);

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
        public IActionResult Delete(string auCode)
        {
            var result = _ACAservices.ClassroomsDelete(auCode);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }







    }
}
