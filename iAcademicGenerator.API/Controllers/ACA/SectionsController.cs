using iAcademicGenerator.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace iAcademicGenerator.API.Controllers.ACA
{
    public class SectionsController : Controller
    {
        private readonly ACAServices _ACAservices;

        public SectionsController(ACAServices acaServices)
        {
            _ACAservices = acaServices ?? throw new ArgumentNullException(nameof(acaServices));
        }
    }
}
