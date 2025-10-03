using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.AUTH

{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly AuthServices _authServices;

        public RolesController(AuthServices AuthServices)
        {
            _authServices = AuthServices ?? throw new ArgumentNullException(nameof(AuthServices));
        }


        [HttpGet("list")]
        public IActionResult List()
        {
            var result = _authServices.ListRoles();

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
        public IActionResult Create([FromBody] RoleCreateDTO role)
        {
            try
            {
                var result = _authServices.CreateRoleWithPermissions(role);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
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


        [HttpPut("update")]
        public IActionResult Update([FromBody] RoleUpdateDTO role)
        {
            try
            {
                var result = _authServices.UpdateRoleWithPermissions(role);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
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


        [HttpGet("{rolCodigo}/permissions")]
        public IActionResult GetRolePermissions(string rolCodigo)
        {
            var result = _authServices.GetRolePermissions(rolCodigo);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }




    }
}