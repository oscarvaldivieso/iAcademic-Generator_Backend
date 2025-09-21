using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI

{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UNIServices _UNIservices;

        public UserController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }

        [HttpGet("list-users")]
        public IActionResult List()
        {
            var result = _UNIservices.ListUsers();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("create-user")]
        public IActionResult Create([FromBody] UserDTO user)
        {

            try
            {
                var result = _UNIservices.UsersInsert(user);

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

        [HttpPost("update-user")]
        public IActionResult Update([FromBody] UserDTO user)
        {

            try
            {
                var result = _UNIservices.UsersUpdate(user);

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
        
        [HttpDelete("delete-user")]
        public IActionResult Delete(string usuCodigo)
        {
            var result = _UNIservices.UsersDelete(usuCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
        
