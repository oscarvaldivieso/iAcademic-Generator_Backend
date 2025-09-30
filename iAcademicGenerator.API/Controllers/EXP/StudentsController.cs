using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.EXP
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly EXPServices _expServices;

        public StudentsController(EXPServices expServices)
        {
            _expServices = expServices ?? throw new ArgumentNullException(nameof(expServices));
        }

        
        [HttpPost("BulkInsert")]
        public IActionResult BulkInsert([FromBody] BulkInsertRequestDTO request)
        {
            // AGREGAR ESTA LÍNEA
            ModelState.Clear(); // Ignora validaciones automáticas
            try
            {
                // Obtener usuario del contexto de autenticación
                // Ajusta según tu implementación de autenticación
                var username = User.Identity?.Name ?? "system";

                var result = _expServices.StudentsBulkInsert(request, username);

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

        
        [HttpPost("ValidateBulk")]
        public IActionResult ValidateBulk([FromBody] BulkInsertRequestDTO request)
        {
            try
            {
                var result = _expServices.ValidateStudentsBulk(request);

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

        // Aquí puedes agregar tus otros endpoints de estudiantes...
        // [HttpGet("List")]
        // [HttpPost("Create")]
        // [HttpPost("Update")]
        // [HttpDelete("Delete")]
    }
}
