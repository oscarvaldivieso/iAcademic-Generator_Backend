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
    public class RequestsRepository
    {

        public IEnumerable<RequestStudentListDTO> List(string est_codigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@est_codigo", est_codigo);

            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);

            var result = db.Query<RequestStudentListDTO>(
                ScriptDatabase.SP_RequestsStudent_List,
                parameter,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return result;
        }

        public RequestStatus RequestAssignmentInsert(RequestAssignmentDTO request)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@pre_codest", request.pre_codest);
            parameter.Add("@created_by", request.created_by);

            // Crear DataTable para el TVP
            var materiasTable = new DataTable();
            materiasTable.Columns.Add("mat_codigo", typeof(string));
            materiasTable.Columns.Add("doc_codigo", typeof(string));
            materiasTable.Columns.Add("mod_codigo", typeof(string));
            materiasTable.Columns.Add("cam_codigo", typeof(string));
            materiasTable.Columns.Add("per_codigo", typeof(string));
            materiasTable.Columns.Add("pre_prioridad", typeof(int));
            materiasTable.Columns.Add("pre_observacion", typeof(string));

            // Llenar el DataTable con las materias
            foreach (var materia in request.Materias)
            {
                materiasTable.Rows.Add(
                    materia.mat_codigo,
                    materia.doc_codigo,
                    materia.mod_codigo,
                    materia.cam_codigo,
                    materia.per_codigo,
                    materia.pre_prioridad,
                    materia.pre_observacion
                );
            }

            // Agregar el parámetro de tipo tabla
            parameter.Add("@Materias", materiasTable.AsTableValuedParameter("[PRE].[TVP_RequestAssignment]"));

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_RequestsInsert, parameter, commandType: CommandType.StoredProcedure);

                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Request assignment inserted successfully"
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
