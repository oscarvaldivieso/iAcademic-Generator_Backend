using iAcademicGenerator.DataAccess.Repositories.UNI;
using iAcademicGenerator.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.BusinessLogic.Services
{
    public class UNIServices
    {
        private readonly CareersRepository _careersRepository;

        public UNIServices(CareersRepository careersRepository)
        {
            _careersRepository = careersRepository;
        }

        #region Careers
        public ServiceResult ListCareers()
        { 
            var result = new ServiceResult();
            try
            {
                var response = _careersRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }

        public ServiceResult CareerInsert(CareersDTO career)
        {
            var result = new ServiceResult();

            try
            {
                // Validaciones básicas antes de llamar al repository
                if (career == null)
                {
                    return result.Error("Career data is required");
                }

                if (string.IsNullOrWhiteSpace(career.car_codigo))
                {
                    return result.Error("Career code is required");
                }

                var response = _careersRepository.CareerInsert(career);

                if (response.CodeStatus == 1)
                {
                    return result.Ok(response);
                }
                else
                {
                    return result.Error(response);
                }
            }
            catch(Exception ex)
            {
                return result.Error($"Unexpected error during career inserting: {ex.Message}");
            }
        }


        public ServiceResult CareerUpdate(CareersDTO career)
        {
            var result = new ServiceResult();

            try
            {
                if (career == null)
                    return result.Error("Career data is required");

                if (string.IsNullOrWhiteSpace(career.car_codigo))
                    return result.Error("Career code is required for update");

                var response = _careersRepository.CareerUpdate(career);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during career updating: {ex.Message}");
            }
        }


        public ServiceResult CareerDelete(string carCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(carCodigo))
                return result.Error("Career code is required for deletion");

            try
            {
                var response = _careersRepository.CareerDelete(carCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during career deletion: {ex.Message}");
            }
        }
        #endregion
    }
}
