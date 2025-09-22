using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models
{
    public class SectionsDTO
    {
        public string sec_codigo { get; set; }
        public int sec_semestre { get; set; }
        public string? sec_destino { get; set; }
        public bool? active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public string mat_codigo { get; set; }
        public string? cam_codigo { get; set; }
    }
}
