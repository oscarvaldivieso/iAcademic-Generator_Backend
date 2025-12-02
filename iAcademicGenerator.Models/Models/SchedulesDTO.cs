using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class SchedulesDTO
    {
        public int hor_codigo { get; set; }
        public string hor_dia_semana { get; set; } = string.Empty;
        public TimeSpan hor_hora_inicio { get; set; }
        public TimeSpan hor_hora_fin { get; set; }
        public int? hor_duracion_minutos { get; set; }
        public bool? active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }
        public string? hor_dia_semana_nombre { get; set; }
    }
}
