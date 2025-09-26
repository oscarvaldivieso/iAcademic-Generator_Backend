using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class CareersDTO
    {
        public string car_codigo { get; set; }
        public string car_nombre { get; set; }
        public int? car_anio_plan { get; set; }
        public string car_orientacion { get; set; }
        public bool active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
    }
}
