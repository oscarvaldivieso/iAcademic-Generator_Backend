using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]
    public class ModalitiesController : Controller
    {
        private readonly UNIServices _UNIservices;

        public ModalitiesController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }


        //This endpoint lists all careers

        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _UNIservices.ListModalities();

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
        public IActionResult Create([FromBody] ModalitiesDTO modality)
        {

            try
            {
                var result = _UNIservices.ModalityInsert(modality);

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
        public IActionResult Update([FromBody] ModalitiesDTO modality)
        {

            try
            {
                var result = _UNIservices.ModalityUpdate(modality);

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
        public IActionResult Delete(string modCodigo)
        {
            var result = _UNIservices.ModalityDelete(modCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
