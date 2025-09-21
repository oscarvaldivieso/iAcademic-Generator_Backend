using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.Models.Models
{
    public class CampusDTO
    {
       public string cam_codigo { get; set; }
       public string cam_nombre { get; set; }
       public string cam_ciudad { get; set; }
       public int active {get; set;}
       public DateTime created_at {get; set;}
       public DateTime updated_at {get; set;}
       public string created_by {get; set;}
       public string updated_by {get; set;}
    }
}
