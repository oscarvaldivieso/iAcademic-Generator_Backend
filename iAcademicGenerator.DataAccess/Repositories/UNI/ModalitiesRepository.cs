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
    public class ModalitiesRepository
    {
        public IEnumerable<ModalitiesDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<ModalitiesDTO>(ScriptDatabase.SP_ModalitiesList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus ModalityInsert(ModalitiesDTO modality)
        {
            var parameter = new DynamicParameters();


            //Input parameters here
            parameter.Add("@mod_codigo", modality.mod_codigo);
            parameter.Add("@mod_nombre", modality.mod_nombre);
            parameter.Add("@created_by", modality.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_ModalityInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Modality inserted succesfully"
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



        public RequestStatus ModalityUpdate(ModalitiesDTO modality)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@mod_codigo", modality.mod_codigo);
            parameter.Add("@mod_nombre", modality.mod_nombre);
            parameter.Add("@updated_by", modality.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ModalityUpdate,
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


        public RequestStatus ModalityDelete(string modCodigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@mod_codigo", modCodigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ModalityDelete,
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
