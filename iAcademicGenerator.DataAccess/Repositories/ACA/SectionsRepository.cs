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
    public class SectionsRepository
    {
        public IEnumerable<SectionsDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<SectionsDTO>(ScriptDatabase.SP_SectionsList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus SectionInsert(SectionsDTO sections)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@sec_codigo", sections.sec_codigo);
            parameter.Add("@sec_semestre", sections.sec_semestre);
            parameter.Add("@sec_destino", sections.@sec_destino);
            parameter.Add("@mat_codigo", sections.mat_codigo);
            parameter.Add("@created_by", sections.created_by);
            parameter.Add("@cam_codigo", sections.cam_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_SectionInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Section inserted succesfully"
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



        public RequestStatus SectionUpdate(SectionsDTO sections)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@sec_codigo", sections.sec_codigo);
            parameter.Add("@sec_semestre", sections.sec_semestre);
            parameter.Add("@sec_destino", sections.@sec_destino);
            parameter.Add("@mat_codigo", sections.mat_codigo);
            parameter.Add("@updated_by", sections.updated_by);
            parameter.Add("@cam_codigo", sections.cam_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SectionUpdate,
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


        public RequestStatus SectionDelete(string sec_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@sec_codigo", sec_codigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_SectionDelete,
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
