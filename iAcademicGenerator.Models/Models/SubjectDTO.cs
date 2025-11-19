using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class SubjectDTO
    {

        public string mat_codigo { get; set; }
        public string mat_nombre { get; set; }

        public int mat_anio { get; set; }
        public int? mat_duracion_clase { get; set; }
        public int? mat_unidades_valorativas { get; set; }
        public string? are_codigo { get; set; }
        public bool mat_es_core { get; set; }
        public bool active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
    }

    public class CareerSubjectsDTO
    {
        public string mat_codigo { get; set; }
        public string mat_nombre { get; set; }
        public int? mat_anio { get; set; }
        public int? mat_unidades_valorativas { get; set; }
        public int? mat_duracion_clase { get; set; }
        public bool? mat_es_core { get; set; }

        public string per_trimestre { get; set; }
        public string car_plan_modulo { get; set; }
        public int? mat_anio_carrera { get; set; }
        public bool? mat_obligatoria { get; set; }
        public int? mat_orden { get; set; }

        public bool cursada { get; set; }
        public string prerrequisitos { get; set; }
        public int total_prerrequisitos { get; set; }
        public bool puede_cursar { get; set; }
        public string prerrequisitos_faltantes { get; set; }
    }

}
