using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess
{
    public interface IDbContext
    {
        IDbConnection CreateConnection();
    }


    public class iAcademicGeneratorContext 
    {
        public static string ConnectionString { get; set; } 

        public iAcademicGeneratorContext() {
        }

        public static void BuildConnectionString(string connection)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder {  ConnectionString = connection };
            ConnectionString = connectionStringBuilder.ConnectionString;
        }

    }
}
