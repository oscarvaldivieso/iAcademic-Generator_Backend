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
    public class ContactsRepository
    {

        public IEnumerable<ContactsDTO> List()
        {
            using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
            var result = db.Query<ContactsDTO>(ScriptDatabase.SP_ContactsList, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return result;
        }

        public RequestStatus ContactInsert(ContactsDTO contacts)
        {
            var parameter = new DynamicParameters();

            parameter.Add("@est_codigo", contacts.est_codigo);
            parameter.Add("@con_telefono_registro", contacts.con_telefono_registro);
            parameter.Add("@con_telefono_portal1", contacts.con_telefono_portal1);
            parameter.Add("@con_telefono_portal2", contacts.con_telefono_portal2);
            parameter.Add("@con_celular_registro", contacts.con_celular_registro);
            parameter.Add("@con_celular_portal", contacts.con_celular_portal);
            parameter.Add("@con_email_universidad", contacts.con_email_universidad);
            parameter.Add("@con_email_registro1", contacts.con_email_registro1);
            parameter.Add("@con_email_registro2", contacts.con_email_registro2); 
            parameter.Add("@con_email_alt_portal1", contacts.con_email_alt_portal1);
            parameter.Add("@con_email_alt_portal2", contacts.con_email_alt_portal2);
            parameter.Add("@created_by", contacts.created_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                db.Execute(ScriptDatabase.SP_ContactInsert, parameter, commandType: CommandType.StoredProcedure);


                return new RequestStatus
                {
                    CodeStatus = 1,
                    MessageStatus = "Contact inserted succesfully"
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



        public RequestStatus ContactUpdate(ContactsDTO contacts)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@contacto_id", contacts.contacto_id);
            parameter.Add("@est_codigo", contacts.est_codigo);
            parameter.Add("@con_telefono_registro", contacts.con_telefono_registro);
            parameter.Add("@con_telefono_portal1", contacts.con_telefono_portal1);
            parameter.Add("@con_telefono_portal2", contacts.con_telefono_portal2);
            parameter.Add("@con_celular_registro", contacts.con_celular_registro);
            parameter.Add("@con_celular_portal", contacts.con_celular_portal);
            parameter.Add("@con_email_universidad", contacts.con_email_universidad);
            parameter.Add("@con_email_registro1", contacts.con_email_registro1);
            parameter.Add("@con_email_registro2", contacts.con_email_registro2);
            parameter.Add("@con_email_alt_portal1", contacts.con_email_alt_portal1);
            parameter.Add("@con_email_alt_portal2", contacts.con_email_alt_portal2);
            parameter.Add("@updated_by", contacts.updated_by);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ContactUpdate,
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


        public RequestStatus ContactDelete(int contacts)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@contacto_id", contacts);

            try
            {
                using var db = new SqlConnection(iAcademicGeneratorContext.ConnectionString);
                var result = db.QueryFirstOrDefault<RequestStatus>(
                    ScriptDatabase.SP_ContactDelete,
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
