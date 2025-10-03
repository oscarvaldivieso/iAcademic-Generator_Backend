using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess.Repositories.AUTH
{
    public class AuthRepository
    {
        public LoginResponseDTO Login(LoginRequestDTO loginRequest)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@user", loginRequest.user);
            parameter.Add("@password", loginRequest.password);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<LoginResponseDTO>(
                    ScriptDatabase.SP_User_Login,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO
                {
                    resultado = "ERROR_SERVIDOR",
                    mensaje = $"Error en el servidor: {ex.Message}"
                };
            }
        }
    }
}
