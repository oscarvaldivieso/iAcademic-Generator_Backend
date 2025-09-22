using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class PeriodsDTO
    {
        public string per_codigo { get; set; }
        public string per_modulo { get; set; }
        public int per_anio { get; set; }
        public DateTime? per_inicio { get; set; }
        public DateTime? per_fin { get; set; }
        public bool active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string created_by { get; set; }
        public string? updated_by { get; set; }
    }
}
