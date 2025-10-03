using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{

    // LoginRequestDTO.cs
    public class LoginRequestDTO
    {
        public string user { get; set; }
        public string password { get; set; }
    }

    // LoginResponseDTO.cs
    public class LoginResponseDTO
    {
        public string resultado { get; set; }
        public string mensaje { get; set; }
        public string usu_codigo { get; set; }
        public string usu_nombre { get; set; }
        public string usu_email { get; set; }
        public string tipo_usuario { get; set; }
        public string est_codigo { get; set; }
        public string doc_codigo { get; set; }
        public string roles { get; set; }
        public DateTime? usu_ultimo_login { get; set; }
        public int? intentos_restantes { get; set; }
    }

    // JwtResponseDTO.cs
    public class JwtResponseDTO
    {
        public string token { get; set; }
        public string usu_codigo { get; set; }
        public string usu_nombre { get; set; }
        public string usu_email { get; set; }
        public string tipo_usuario { get; set; }
        public string roles { get; set; }
        public DateTime expires_at { get; set; }
    }
}
