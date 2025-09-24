using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI

{
    [Route("[controller]")]
    [ApiController]
    public class RolController : Controller
    {
        private readonly UNIServices _UNIservices;

        public RolController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }

        
        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _UNIservices.ListRol();

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
        public IActionResult Create([FromBody] RolDTO rol)
        {

            try
            {
                var result = _UNIservices.RolInsert(rol);

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
        public IActionResult Update([FromBody] RolDTO rol)
        {

            try
            {
                var result = _UNIservices.RolUpdate(rol);

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
        public IActionResult Delete(string rolCodigo)
        {
            var result = _UNIservices.RolDelete(rolCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}