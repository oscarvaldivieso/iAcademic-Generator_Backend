using iAcademicGenerator.DataAccess.Repositories.ACA;
using iAcademicGenerator.DataAccess.Repositories.EXP;
using iAcademicGenerator.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAcademicGenerator.BusinessLogic.Services
{
    public class EXPServices
    {

        private readonly ContactsRepository _contactsRepository;

        public EXPServices(ContactsRepository contactsRepository )
        {
         
            _contactsRepository = contactsRepository;
        }

        #region Contacts
        public ServiceResult ListContacts()
        {
            var result = new ServiceResult();
            try
            {
                var response = _contactsRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }

        public ServiceResult ContactInsert(ContactsDTO contacts)
        {
            var result = new ServiceResult();
            try
            {
                var response = _contactsRepository.ContactInsert(contacts);
                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response.MessageStatus);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult ContactUpdate(ContactsDTO contacts)
        {
            var result = new ServiceResult();
            try
            {
                var response = _contactsRepository.ContactUpdate(contacts);
                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response.MessageStatus);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }

        public ServiceResult ContactsDelete(int id)
        {
            var result = new ServiceResult();

            if (id == null)
                return result.Error("contacts code is required for deletion");

            try
            {
                var response = _contactsRepository.ContactDelete(id);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during contacts deletion: {ex.Message}");
            }
        }

        #endregion



    }
}
