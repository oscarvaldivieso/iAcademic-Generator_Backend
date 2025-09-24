using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iAcademicGenerator.DataAccess.Repositories.UNI
{
    public class CampusRepository
    {
        public IEnumerable<CampusDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<CampusDTO>(
                ScriptDatabase.SP_CampusList,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus CampusInsert(CampusDTO campus)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@cam_codigo", campus.cam_codigo);
            parameter.Add("@cam_nombre", campus.cam_nombre);
            parameter.Add("@cam_ciudad", campus.cam_ciudad);
            parameter.Add("@created_by", campus.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_CampusInsert, parameter, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Campus inserted successfully"
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

        public RequestStatus CampusUpdate(CampusDTO campus)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@cam_codigo", campus.cam_codigo);
            parameter.Add("@cam_nombre", campus.cam_nombre);
            parameter.Add("@cam_ciudad", campus.cam_ciudad);
            parameter.Add("@updated_by", campus.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_CampusUpdate,
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

        public RequestStatus CampusDelete(string cam_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@cam_codigo", cam_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_CampusDelete,
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

