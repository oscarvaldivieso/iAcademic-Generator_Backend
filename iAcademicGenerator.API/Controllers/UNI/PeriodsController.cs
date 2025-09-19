using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]
    public class PeriodsController : Controller
    {
        private readonly UNIServices _UNIservices;

        public PeriodsController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }


        //This endpoint lists all careers

        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _UNIservices.ListPeriods();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        //This endpoint insert a new career
        [HttpPost("create")]
        public IActionResult Create([FromBody] PeriodsDTO period)
        {

            try
            {
                var result = _UNIservices.PeriodInsert(period);

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
        public IActionResult Update([FromBody] PeriodsDTO period)
        {

            try
            {
                var result = _UNIservices.PeriodUpdate(period);

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
        public IActionResult Delete(string perCodigo)
        {
            var result = _UNIservices.PeriodDelete(perCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
    
}
