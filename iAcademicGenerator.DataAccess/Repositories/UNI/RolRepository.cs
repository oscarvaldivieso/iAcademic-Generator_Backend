using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iAcademicGenerator.DataAccess.Repositories.UNI
{
    public class RolRepository
    {
        public IEnumerable<RolDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<RolDTO>(
                ScriptDatabase.SP_UserRolesList,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus RolInsert(RolDTO userRole)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@usu_codigo", userRole.usu_codigo);
            parameter.Add("@rol_codigo", userRole.rol_codigo);
            parameter.Add("@active", userRole.active);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_UserRoleInsert, parameter, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "User role inserted successfully"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public RequestStatus RolUpdate(RolDTO userRole)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@usu_codigo", userRole.usu_codigo);
            parameter.Add("@rol_codigo", userRole.rol_codigo);
            parameter.Add("@active", userRole.active);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_UserRoleUpdate,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Unknown error during update"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public RequestStatus RolDelete( string rol_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@rol_codigo", rol_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_UserRoleDelete,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Unknown error during deletion"
                };
            }
            catch (Exception ex)
            {
                return new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = $"Unexpected error: {ex.Message}"
                };
            }
        }
    }
}
