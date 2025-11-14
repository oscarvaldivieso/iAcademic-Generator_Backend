using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class AcademicOfferDTO
    {
    }

    public class OfferListDTO
    {
        public string Codigo { get; set; }
        public string Materia { get; set; }
        public string Dias { get; set; }
        public string Bloque { get; set; }
        public string Hora { get; set; }
        public int Cupos { get; set; }
        public string Campus { get; set; }
        public string Seccion { get; set; }
        public string Docente { get; set; }
    }
}
