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
    public class PeriodsRepository
    {
        public IEnumerable<PeriodsDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<PeriodsDTO>(ScriptDatabase.SP_PeriodsList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus PeriodInsert(PeriodsDTO period)
        {
            var parameter = new DynamicParameters();


            //Input parameters here
            parameter.Add("@per_codigo", period.per_codigo);
            parameter.Add("@per_trimestre", period.per_trimestre);
            parameter.Add("@per_anio", period.per_anio);
            parameter.Add("@per_inicio", period.per_inicio);
            parameter.Add("@per_fin", period.per_fin);
            parameter.Add("@created_by", period.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_PeriodInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Period inserted succesfully"
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



        public RequestStatus PeriodUpdate(PeriodsDTO period)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@per_codigo", period.per_codigo);
            parameter.Add("@per_trimestre", period.per_trimestre);
            parameter.Add("@per_anio", period.per_anio);
            parameter.Add("@per_inicio", period.per_inicio);
            parameter.Add("@per_fin", period.per_fin);
            parameter.Add("@updated_by", period.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_PeriodUpdate,
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


        public RequestStatus PeriodDelete(string perCodigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@per_codigo", perCodigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_PeriodDelete,
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
