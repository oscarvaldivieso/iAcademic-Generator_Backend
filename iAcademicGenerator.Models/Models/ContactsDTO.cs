using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class ContactsDTO
    {
        public int contacto_id { get; set; }
        public string est_codigo { get; set; }
        public bool active { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? created_by { get; set; }
        public string? updated_by { get; set; }

        public string? con_telefono_registro { get; set; }
        public string? con_telefono_portal1 { get; set; }    
        public string? con_telefono_portal2 { get; set; }
        public string? con_celular_registro { get; set; }
        public string? con_celular_portal { get; set; }    
        public string? con_email_universidad { get; set; }    
        public string? con_email_registro1 { get; set; }
          public string? con_email_registro2 { get; set; }    
        public string? con_email_alt_portal1 { get; set; }    
        public string? con_email_alt_portal2 { get; set; }



    }
}
