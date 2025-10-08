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
    public class SubjectsRepository
    {

        public IEnumerable<SubjectDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<SubjectDTO>(ScriptDatabase.SP_SubjectsList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus SubjectInsert(SubjectDTO subjects)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@mat_codigo", subjects.mat_codigo);  
            parameter.Add("@mat_nombre", subjects.mat_nombre);
            parameter.Add("@mat_es_core", subjects.mat_es_core);
            parameter.Add("@are_codigo", subjects.are_codigo);
            parameter.Add("@created_by", subjects.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_SubjectInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Subject inserted succesfully"
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



        public RequestStatus SubjectUpdate(SubjectDTO subjects)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@mat_codigo", subjects.mat_codigo);
            parameter.Add("@mat_nombre", subjects.mat_nombre);
            parameter.Add("@mat_es_core", subjects.mat_es_core);
            parameter.Add("@are_codigo", subjects.are_codigo);
            parameter.Add("@updated_by", subjects.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SubjectUpdate,
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


        public RequestStatus SubjectDelete(string mat_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@mat_codigo", mat_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SubjectDelete,
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
