using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.EXP
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly EXPServices _EXPservices;

        public ContactsController(EXPServices expServices)
        {
            _EXPservices = expServices ?? throw new ArgumentNullException(nameof(expServices));
        }



        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _EXPservices.ListContacts();

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
        public IActionResult Create([FromBody] ContactsDTO contacts)
        {

            try
            {
                var result = _EXPservices.ContactInsert(contacts);

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
        public IActionResult Update([FromBody] ContactsDTO contacts)
        {

            try
            {
                var result = _EXPservices.ContactUpdate(contacts);

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
            var result = _EXPservices.ContactsDelete(id);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }







    }
}
