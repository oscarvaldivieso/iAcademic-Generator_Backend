using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class TeachersDTO
    {

        public string doc_codigo { get; set; }
        public string doc_nombre { get; set; }
   
        public bool active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public string gru_codigo { get; set; }


    }

    public class TeacherBulkDTO
    {
        public string doc_codigo { get; set; } = string.Empty;
        public string doc_nombre { get; set; } = string.Empty;
        public string gru_codigo { get; set; } = string.Empty;
    }

    public class TeachersBulkInsertRequestDTO
    {
        public List<TeacherBulkDTO> Docentes { get; set; } = new();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class TeachersBulkInsertResponseDTO
    {
        public bool Success { get; set; }
        public int InsertadosCount { get; set; }
        public int ActualizadosCount { get; set; }
        public int ErroresCount { get; set; }
        public List<TeacherErrorDetailDTO> Errores { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    public class TeacherErrorDetailDTO
    {
        public string doc_codigo { get; set; } = string.Empty;
        public string mensaje_error { get; set; } = string.Empty;
    }

}
