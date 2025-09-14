using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess
{
    // Enums para códigos de estado más específicos
    

    // RequestStatus mejorado
    public class RequestStatus
    {
        public int CodeStatus { get; set; }
        public string MessageStatus { get; set; }

        public object Data { get; set; }
    }
}
