using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectsController : Controller
    {
        private readonly ACAServices _ACAservices;

        public SubjectsController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }



        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _ACAservices.ListSubjects();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpGet("career-subjects/{est_codigo}/{car_codigo}")]
        public IActionResult ListCareerSubjects(string est_codigo, string car_codigo)
        {
            if (string.IsNullOrWhiteSpace(est_codigo) || string.IsNullOrWhiteSpace(car_codigo))
                return BadRequest("Student code and career code are required.");

            var result = _ACAservices.ListCareerSubjects(est_codigo, car_codigo);
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] SubjectDTO subjects)
        {

            try
            {
                var result = _ACAservices.SubjectsInsert(subjects);

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
        public IActionResult Update([FromBody] SubjectDTO subjects)
        {

            try
            {
                var result = _ACAservices.SubjectsUpdate(subjects);

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
        public IActionResult Delete(string subCode)
        {
            var result = _ACAservices.SubjectsDelete(subCode);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }







    }
}
