using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class RequestDTO
    {
        public string pre_codest { get; set; }
        public string created_by { get; set; }

    }

    // DTO para cada materia en la solicitud
    public class RequestAssignmentItemDTO
    {
        public string mat_codigo { get; set; }

        public int hor_codigo { get; set; }

        public string doc_codigo { get; set; }
        public string mod_codigo { get; set; }
        public string cam_codigo { get; set; }
        public string per_codigo { get; set; }
        public int pre_prioridad { get; set; }
        public string pre_observacion { get; set; }
    }

    // DTO principal para la solicitud
    public class RequestAssignmentDTO
    {
        public string pre_codest { get; set; }
        public string created_by { get; set; }
        public List<RequestAssignmentItemDTO> Materias { get; set; }
    }


    public class RequestStudentListDTO
    {
        public int pre_codigo { get; set; }
        public string pre_codest { get; set; }
        public string pre_estado { get; set; }
        public DateTime? pre_fecha { get; set; }
        public string pre_observacion { get; set; }

        public string mat_codigo { get; set; }
        public string mat_nombre { get; set; }

        public string doc_codigo { get; set; }
        public string doc_nombre { get; set; }

        public string mod_codigo { get; set; }
        public string mod_nombre { get; set; }

        public string cam_codigo { get; set; }
        public string cam_nombre { get; set; }

        public string per_codigo { get; set; }
        public string periodo { get; set; }

        public int? pre_prioridad { get; set; }

        public int? hor_codigo { get; set; }
        public string hor_dia_semana_nombre { get; set; }

        public DateTime? created_at { get; set; }
    }
}
