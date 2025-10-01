using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class StudentsDTO
    {
        public string est_codigo { get; set; }
        public string est_nombre { get; set; }
        public string est_genero { get; set; }
        public decimal? est_indice_general { get; set; }
        public decimal? est_indice_graduacion { get; set; }
        public string car_codigo { get; set; }

        [NotMapped]
        public string car_nombre { get; set; }
        public bool? active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public string cam_codigo { get; set; }
        [NotMapped]
        public string cam_nombre { get; set; }
        public string gru_codigo { get; set; }
        [NotMapped]
        public string gru_nombre { get; set; }
    }

    public class StudentBulkDTO
    {
        public string est_codigo { get; set; }
        public string est_nombre { get; set; }
        public string? est_genero { get; set; }
        public decimal? est_indice_general { get; set; }
        public decimal? est_indice_graduacion { get; set; }
        public string car_codigo { get; set; }
        public string cam_codigo { get; set; }
        public string? gru_codigo { get; set; }

        // Datos opcionales para crear carrera si no existe
        public string car_nombre { get; set; }
        public int? car_anio_plan { get; set; }
        public string? car_orientacion { get; set; }

        // Datos opcionales para crear campus si no existe
        public string cam_nombre { get; set; }
        public string cam_ciudad { get; set; }
    }

    public class BulkInsertRequestDTO
    {
        public List<StudentBulkDTO> Estudiantes { get; set; }
    }

    public class BulkInsertResponseDTO
    {
        public bool Success { get; set; }
        public int EstudiantesInsertados { get; set; }
        public int EstudiantesActualizados { get; set; }
        public int ErroresEncontrados { get; set; }
        public int CarrerasCreadas { get; set; }
        public int CampusCreados { get; set; }
        public List<ErrorDetailDto> Errores { get; set; }
        public string Message { get; set; }
    }

    public class ErrorDetailDto
    {
        public string est_codigo { get; set; }
        public string mensaje_error  { get; set; }
    }
}
