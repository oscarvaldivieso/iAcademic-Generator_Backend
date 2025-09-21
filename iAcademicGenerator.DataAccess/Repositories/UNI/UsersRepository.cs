using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iAcademicGenerator.DataAccess.Repositories.UNI
{
    public class UsersRepository
    {
        public IEnumerable<UserDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<UserDTO>(
                ScriptDatabase.SP_UsersList,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus UsersInsert(UserDTO user)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@usu_codigo", user.usu_codigo);
            parameter.Add("@usu_nombre", user.usu_nombre);
            parameter.Add("@usu_password", user.usu_password);
            parameter.Add("@usu_email", user.usu_email);
            parameter.Add("@usu_activo", user.usu_activo);
            parameter.Add("@usu_ultimo_login", user.usu_ultimo_login);
            parameter.Add("@usu_intentos_fallidos", user.usu_intentos_fallidos);
            parameter.Add("@usu_bloqueado", user.usu_bloqueado);
            parameter.Add("@active", user.active);
            parameter.Add("@create_at", user.create_at);
            parameter.Add("@update_at", user.update_at);
            parameter.Add("@create_by", user.create_by);
            parameter.Add("@update_by", user.update_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_UserInsert, parameter, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "User inserted successfully"
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

        public RequestStatus UsersUpdate(UserDTO user)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@usu_codigo", user.usu_codigo);
            parameter.Add("@usu_nombre", user.usu_nombre);
            parameter.Add("@usu_password", user.usu_password);
            parameter.Add("@usu_email", user.usu_email);
            parameter.Add("@usu_activo", user.usu_activo);
            parameter.Add("@usu_ultimo_login", user.usu_ultimo_login);
            parameter.Add("@usu_intentos_fallidos", user.usu_intentos_fallidos);
            parameter.Add("@usu_bloqueado", user.usu_bloqueado);
            parameter.Add("@active", user.active);
            parameter.Add("@update_at", user.update_at);
            parameter.Add("@update_by", user.update_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_UserUpdate,
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

        public RequestStatus UsersDelete(string usu_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@usu_codigo", usu_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_UserDelete,
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
