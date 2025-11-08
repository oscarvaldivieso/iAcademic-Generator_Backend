using iAcademicGenerator.BusinessLogic.Services;
using iAcademicGenerator.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    [Route("[controller]")]
    [ApiController]
    public class SchedulesController : Controller
    {
        private readonly ACAServices _ACAservices;

        public SchedulesController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _ACAservices.ListSchedules();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] SchedulesDTO schedule)
        {
            var result = _ACAservices.ScheduleInsert(schedule);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] SchedulesDTO schedule)
        {
            var result = _ACAservices.ScheduleUpdate(schedule);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int horCodigo)
        {
            var result = _ACAservices.ScheduleDelete(horCodigo);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
