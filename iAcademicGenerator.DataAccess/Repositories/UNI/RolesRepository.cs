using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iAcademicGenerator.DataAccess.Repositories.UNI
{
    public class RolesRepository
    {
        public IEnumerable<RolesDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<RolesDTO>(
                ScriptDatabase.SP_RolList,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus RolesInsert(RolesDTO rol)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@rol_nombre", rol.rol_nombre);
            parameter.Add("@active", rol.active);
            parameter.Add("@create_at", rol.create_at);
            parameter.Add("@update_at", rol.update_at);
            parameter.Add("@create_by", rol.create_by);
            parameter.Add("@update_by", rol.update_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_RolInsert, parameter, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Role inserted successfully"
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

        public RequestStatus RolesUpdate(RolesDTO rol)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@rol_codigo", rol.rol_codigo);
            parameter.Add("@rol_nombre", rol.rol_nombre);
            parameter.Add("@active", rol.active);
            parameter.Add("@update_at", rol.update_at);
            parameter.Add("@update_by", rol.update_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_RolUpdate,
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

        public RequestStatus RolesDelete(string rol_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@rol_codigo", rol_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_RolDelete,
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
