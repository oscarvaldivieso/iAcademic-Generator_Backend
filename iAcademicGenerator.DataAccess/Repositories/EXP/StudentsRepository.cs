using Dapper;
using iAcademicGenerator.Models.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess.Repositories.EXP
{
    public class StudentsRepository
    {

        private DataTable CreateStudentsBulkDataTable(List<StudentBulkDTO> estudiantes)
        {
            var dataTable = new DataTable();

            // Definir columnas
            dataTable.Columns.Add("est_codigo", typeof(string));
            dataTable.Columns.Add("est_nombre", typeof(string));
            dataTable.Columns.Add("est_genero", typeof(string));
            dataTable.Columns.Add("est_indice_general", typeof(decimal));
            dataTable.Columns.Add("est_indice_graduacion", typeof(decimal));
            dataTable.Columns.Add("car_codigo", typeof(string));
            dataTable.Columns.Add("cam_codigo", typeof(string));
            dataTable.Columns.Add("gru_codigo", typeof(string));
            dataTable.Columns.Add("car_nombre", typeof(string));
            dataTable.Columns.Add("car_anio_plan", typeof(int));
            dataTable.Columns.Add("car_orientacion", typeof(string));
            dataTable.Columns.Add("cam_nombre", typeof(string));
            dataTable.Columns.Add("cam_ciudad", typeof(string));

            // Llenar filas
            foreach (var est in estudiantes)
            {
                dataTable.Rows.Add(
                    est.est_codigo,
                    est.est_nombre,
                    est.est_genero ?? (object)DBNull.Value,
                    est.est_indice_general ?? (object)DBNull.Value,
                    est.est_indice_graduacion ?? (object)DBNull.Value,
                    est.car_codigo,
                    est.cam_codigo ?? (object)DBNull.Value,
                    est.gru_codigo ?? (object)DBNull.Value,
                    est.car_nombre ?? (object)DBNull.Value,
                    est.car_anio_plan ?? (object)DBNull.Value,
                    est.car_orientacion ?? (object)DBNull.Value,
                    est.cam_nombre ?? (object)DBNull.Value,
                    est.cam_ciudad ?? (object)DBNull.Value
                );
            }

            return dataTable;
        }

        public BulkInsertResponseDTO Students_BulkInsert(BulkInsertRequestDTO request, string createdBy)
        {
            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);

                // Crear DataTable
                var dataTable = CreateStudentsBulkDataTable(request.Estudiantes);

                // Preparar parámetros con Dapper
                var parameters = new DynamicParameters();
                parameters.Add("@Estudiantes", dataTable.AsTableValuedParameter("EXP.TVP_StudentsBulk"));
                parameters.Add("@CreatedBy", createdBy);
                parameters.Add("@InsertadosCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ActualizadosCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ErroresCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@CarrerasCreadas", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@CampusCreados", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Ejecutar y capturar errores
                var errores = db.Query<ErrorDetailDto>(
                    ScriptDatabase.SP_StudentsBulkInsert,
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                // Obtener valores de salida
                int insertados = parameters.Get<int>("@InsertadosCount");
                int actualizados = parameters.Get<int>("@ActualizadosCount");
                int erroresCount = parameters.Get<int>("@ErroresCount");
                int carreras = parameters.Get<int>("@CarrerasCreadas");
                int campus = parameters.Get<int>("@CampusCreados");

                // Construir respuesta
                var response = new BulkInsertResponseDTO
                {
                    EstudiantesInsertados = insertados,
                    EstudiantesActualizados = actualizados,
                    ErroresEncontrados = erroresCount,
                    CarrerasCreadas = carreras,
                    CampusCreados = campus,
                    Errores = errores,
                    Success = erroresCount == 0
                };

                // Mensaje descriptivo
                var totalProcesados = insertados + actualizados;
                response.Message = $"Procesados: {totalProcesados} estudiantes. " +
                                 $"Insertados: {insertados}, Actualizados: {actualizados}";

                if (erroresCount > 0)
                    response.Message += $", Errores: {erroresCount}";

                if (carreras > 0)
                    response.Message += $". Se crearon {carreras} carrera(s)";

                if (campus > 0)
                    response.Message += $". Se crearon {campus} campus";

                return response;
            }
            catch (Exception ex)
            {
                return new BulkInsertResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Errores = new List<ErrorDetailDto>()
                };
            }
        }

        public IEnumerable<StudentsDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<StudentsDTO>(ScriptDatabase.SP_StudentsList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }


        public RequestStatus StudentInsert(StudentsDTO students)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@est_codigo", students.est_codigo);
            parameter.Add("@est_nombre", students.est_nombre);
            parameter.Add("@est_genero", students.est_genero);
            parameter.Add("@est_indice_general", students.est_indice_general);
            parameter.Add("@est_indice_graduacion", students.est_indice_graduacion);
            parameter.Add("@car_codigo", students.car_codigo);
            parameter.Add("@cam_codigo", students.cam_codigo);
            parameter.Add("@gru_codigo", students.gru_codigo);
            parameter.Add("@created_by", students.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_StudentInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Student inserted succesfully"
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



        public RequestStatus StudentUpdate(StudentsDTO students)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@est_codigo", students.est_codigo);
            parameter.Add("@est_nombre", students.est_nombre);
            parameter.Add("@est_genero", students.est_genero);
            parameter.Add("@est_indice_general", students.est_indice_general);
            parameter.Add("@est_indice_graduacion", students.est_indice_graduacion);
            parameter.Add("@car_codigo", students.car_codigo);
            parameter.Add("@cam_codigo", students.cam_codigo);
            parameter.Add("@gru_codigo", students.gru_codigo);
            parameter.Add("@updated_by", students.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_StudentUpdate,
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


        public RequestStatus StudentDelete(int student)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@est_codigo", student);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_StudentDelete,
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
