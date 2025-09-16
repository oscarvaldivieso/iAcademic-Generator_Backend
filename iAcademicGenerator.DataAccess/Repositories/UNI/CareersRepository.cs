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
    public class CareersRepository
    {
        public IEnumerable<CareersDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<CareersDTO>(ScriptDatabase.SP_CareersList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus CareerInsert(CareersDTO career)
        {
            var parameter = new DynamicParameters();


            //Input parameters here
            parameter.Add("@car_codigo", career.car_codigo);
            parameter.Add("@car_nombre", career.car_nombre);
            parameter.Add("@car_anio_plan", career.car_anio_plan);
            parameter.Add("@car_orientacion", career.car_orientacion);
            parameter.Add("@created_by", career.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_CareerInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus =  1 ,
                    MessageStatus = "Career inserted succesfully"
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



        public RequestStatus CareerUpdate(CareersDTO career)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@car_codigo", career.car_codigo);
            parameter.Add("@car_nombre", career.car_nombre);
            parameter.Add("@car_anio_plan", career.car_anio_plan);
            parameter.Add("@car_orientacion", career.car_orientacion);
            parameter.Add("@updated_by", career.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_CareerUpdate,
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


        public RequestStatus CareerDelete(string carCodigo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@car_codigo", carCodigo);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_CareerDelete,
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
