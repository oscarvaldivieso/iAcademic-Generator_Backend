using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI

{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly UNIServices _UNIservices;

        public RolesController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }


        [HttpGet("list-roles")]
        public IActionResult List()
        {
            var result = _UNIservices.ListRoles();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        
        [HttpPost("create-roles")]
        public IActionResult Create([FromBody] RolesDTO roles)
        {

            try
            {
                var result = _UNIservices.RolesInsert(roles);

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
        [HttpPost("update-roles")]
        public IActionResult Update([FromBody] RolesDTO roles)
        {

            try
            {
                var result = _UNIservices.RolesUpdate(roles);

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
        
        [HttpDelete("delete-roles")]
        public IActionResult Delete(string rolesCodigo)
        {
            var result = _UNIservices.RolesDelete(rolesCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}