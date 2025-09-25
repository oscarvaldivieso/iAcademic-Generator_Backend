using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class TeachersController : Controller
    {
        private readonly ACAServices _ACAservices;

        public TeachersController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }



        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _ACAservices.ListTeachers();

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
        public IActionResult Create([FromBody] TeachersDTO teachers)
        {

            try
            {
                var result = _ACAservices.TeachersInsert(teachers);

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
        public IActionResult Update([FromBody] TeachersDTO teachers)
        {

            try
            {
                var result = _ACAservices.TeachersUpdate(teachers);

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
        public IActionResult Delete(int id)
        {
            var result = _ACAservices.TeachersDelete(id);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }







    }
}
