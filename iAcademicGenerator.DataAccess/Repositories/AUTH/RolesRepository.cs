using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iAcademicGenerator.DataAccess.Repositories.AUTH
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


        // Crear rol con permisos
        public RequestStatus CreateRoleWithPermissions(RoleCreateDTO role)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@rol_codigo", role.rol_codigo);
            parameter.Add("@rol_nombre", role.rol_nombre);
            // Convertir la lista de permisos a string separado por comas
            parameter.Add("@permisos", role.permisos != null && role.permisos.Any()
                ? string.Join(",", role.permisos)
                : null);
            parameter.Add("@created_by", role.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Rol_CreateWithPermissions,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Unknown error during role creation"
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

        // Actualizar rol con permisos (todo en uno)
        public RequestStatus UpdateRoleWithPermissions(RoleUpdateDTO role)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@rol_codigo", role.rol_codigo);
            parameter.Add("@rol_nombre", role.rol_nombre);
            parameter.Add("@permisos", role.permisos != null && role.permisos.Any()
                ? string.Join(",", role.permisos)
                : null);
            parameter.Add("@updated_by", role.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_Roles_UpdateWithPermissions,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new RequestStatus
                {
                    CodeStatus = 0,
                    MessageStatus = "Unknown error during role update"
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


        public IEnumerable<PermissionWithStatusDTO> GetPermissionsWithStatus(string rolCodigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@rol_codigo", rolCodigo);

            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<PermissionWithStatusDTO>(
                ScriptDatabase.SP_Roles_GetPermissionsWithStatus,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
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
