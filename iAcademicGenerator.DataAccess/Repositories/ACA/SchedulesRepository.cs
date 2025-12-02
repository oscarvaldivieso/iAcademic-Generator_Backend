using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess.Repositories.ACA
{
    public class SchedulesRepository
    {
        public IEnumerable<SchedulesDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<SchedulesDTO>(ScriptDatabase.SP_SchedulesList, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public RequestStatus ScheduleInsert(SchedulesDTO schedule)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@hor_dia_semana", schedule.hor_dia_semana);
            parameters.Add("@hor_hora_inicio", schedule.hor_hora_inicio);
            parameters.Add("@hor_hora_fin", schedule.hor_hora_fin);
            parameters.Add("@hor_duracion_minutos", schedule.hor_duracion_minutos);
            parameters.Add("@hor_dia_semana_nombre", schedule.hor_dia_semana_nombre);
            parameters.Add("@created_by", schedule.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_ScheduleInsert, parameters, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Schedule inserted successfully"
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

        public RequestStatus ScheduleUpdate(SchedulesDTO schedule)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@hor_codigo", schedule.hor_codigo);
            parameters.Add("@hor_dia_semana", schedule.hor_dia_semana);
            parameters.Add("@hor_hora_inicio", schedule.hor_hora_inicio);
            parameters.Add("@hor_hora_fin", schedule.hor_hora_fin);
            parameters.Add("@hor_duracion_minutos", schedule.hor_duracion_minutos);
            parameters.Add("@hor_dia_semana_nombre", schedule.hor_dia_semana_nombre);
            parameters.Add("@updated_by", schedule.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ScheduleUpdate,
                    parameters,
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

        public RequestStatus ScheduleDelete(int horCodigo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@hor_codigo", horCodigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ScheduleDelete,
                    parameters,
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
