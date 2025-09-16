using iAcademicGenerator.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.UNI
{
    [Route("[controller]")]
    [ApiController]
    public class CampusController : Controller
    {
        private readonly UNIServices _UNIservices;

        public CampusController(UNIServices uniServices)
        {
            _UNIservices = uniServices ?? throw new ArgumentNullException(nameof(uniServices));
        }
    }
}
