using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess.Repositories.UNI
{
    public class OffersRepository
    {

        public IEnumerable<OfferListDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);

            var result = db.Query<OfferListDTO>(
                ScriptDatabase.SP_OfferList,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }
    }
}
