using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.BusinessLogic
{
    public enum ServiceResultType
    {
        Success = 200,
        Created = 201,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500,
        DatabaseError = 501,
        ValidationError = 422
    }
}
