using AutoMapper;
using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;
using iAcademicGenerator.Models.Models;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]
    public class CareersController : Controller
    {
        private readonly UNIServices _UNIservices;

        public CareersController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }




        //This endpoint lists all careers

        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _UNIservices.ListCareers();

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
        public IActionResult Create([FromBody] CareersDTO career)
        {

            try
            {
                var result = _UNIservices.CareerInsert(career);

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
        public IActionResult Update([FromBody] CareersDTO career)
        {

            try
            {
                var result = _UNIservices.CareerUpdate(career);

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
        public IActionResult Delete(string carCodigo)
        {
            var result = _UNIservices.CareerDelete(carCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
