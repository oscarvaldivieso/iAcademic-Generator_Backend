using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class ModalitiesDTO
    {
        public string mod_codigo { get; set; }
        public string mod_nombre { get; set; }
        public bool? active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string created_by { get; set; }
        public string? updated_by { get; set; }
    }
}
