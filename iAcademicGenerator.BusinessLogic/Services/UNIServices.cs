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
        private readonly CampusRepository _campusRepository;
        private readonly UsersRepository _usersRepository;
        private readonly RolRepository _rolRepository;
        private readonly RolesRepository _rolesRepository;
        public UNIServices(CareersRepository careersRepository, CampusRepository campusRepository, UsersRepository usersRepository, 
            RolRepository rolRepository,RolesRepository rolesRepository)
        {
            _careersRepository = careersRepository;
            _campusRepository = campusRepository;
            _usersRepository = usersRepository;
            _rolRepository = rolRepository;
            _rolesRepository = rolesRepository;
        }

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
            catch (Exception ex)
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
        
        public ServiceResult ListUsers()
        {
            var result = new ServiceResult();
            try
            {
                var response = _usersRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult UsersInsert(UserDTO user)
        {
            var result = new ServiceResult();

            try
            {
                if (user == null)
                {
                    return result.Error("User data is required");
                }

                if (string.IsNullOrWhiteSpace(user.usu_codigo))
                {
                    return result.Error("User code is required");
                }

                var response = _usersRepository.UsersInsert(user);

                if (response.CodeStatus == 1)
                {
                    return result.Ok(response);
                }
                else
                {
                    return result.Error(response);
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during user inserting: {ex.Message}");
            }
        }
        public ServiceResult UsersUpdate(UserDTO user)
        {
            var result = new ServiceResult();

            try
            {
                if (user == null)
                    return result.Error("User data is required");

                if (string.IsNullOrWhiteSpace(user.usu_codigo))
                    return result.Error("User code is required for update");

                var response = _usersRepository.UsersUpdate(user);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during user updating: {ex.Message}");
            }
        }
        public ServiceResult UsersDelete(string usuCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(usuCodigo))
                return result.Error("User code is required for deletion");

            try
            {
                var response = _usersRepository.UsersDelete(usuCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during user deletion: {ex.Message}");
            }
        }

        public ServiceResult ListRol()
        {
            var result = new ServiceResult();
            try
            {
                var response = _rolRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult RolInsert(RolDTO rol)
        {
            var result = new ServiceResult();

            try
            {
                if (rol == null)
                {
                    return result.Error("Rol data is required");
                }

                if (string.IsNullOrWhiteSpace(rol.rol_codigo))
                {
                    return result.Error("Rol code is required");
                }

                var response = _rolRepository.RolInsert(rol);

                if (response.CodeStatus == 1)
                {
                    return result.Ok(response);
                }
                else
                {
                    return result.Error(response);
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol inserting: {ex.Message}");
            }
        }
        
        public ServiceResult RolUpdate(RolDTO rol)
        {
            var result = new ServiceResult();

            try
            {
                if (rol == null)
                    return result.Error("Rol data is required");

                if (string.IsNullOrWhiteSpace(rol.rol_codigo))
                    return result.Error("Rol code is required for update");

                var response = _rolRepository.RolUpdate(rol);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol updating: {ex.Message}");
            }
        }
        public ServiceResult RolDelete(string rolCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(rolCodigo))
                return result.Error("Rol code is required for deletion");

            try
            {
                var response = _rolRepository.RolDelete(rolCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol deletion: {ex.Message}");
            }
        }
        public ServiceResult ListRoles()
        {
            var result = new ServiceResult();
            try
            {
                var response = _rolesRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult RolesInsert(RolesDTO roles)
        {
            var result = new ServiceResult();

            try
            {
                if (roles == null)
                {
                    return result.Error("Rol data is required");
                }

                if (string.IsNullOrWhiteSpace(roles.rol_codigo))
                {
                    return result.Error("Rol code is required");
                }

                var response = _rolesRepository.RolesInsert(roles);

                if (response.CodeStatus == 1)
                {
                    return result.Ok(response);
                }
                else
                {
                    return result.Error(response);
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol inserting: {ex.Message}");
            }
        }
        
        public ServiceResult RolesUpdate(RolesDTO roles)
        {
            var result = new ServiceResult();

            try
            {
                if (roles == null)
                    return result.Error("Rol data is required");

                if (string.IsNullOrWhiteSpace(roles.rol_codigo))
                    return result.Error("Rol code is required for update");

                var response = _rolesRepository.RolesUpdate(roles);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol updating: {ex.Message}");
            }
        }
        
        public ServiceResult RolesDelete(string rolesCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(rolesCodigo))
                return result.Error("Rol code is required for deletion");

            try
            {
                var response = _rolesRepository.RolesDelete(rolesCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol deletion: {ex.Message}");
            }
        }
        
        public ServiceResult ListCampus()
        {
            var result = new ServiceResult();
            try
            {
                var response = _campusRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult CampusInsert(CampusDTO campus)
        {
            var result = new ServiceResult();

            try
            {
                if (campus == null)
                {
                    return result.Error("Rol data is required");
                }

                if (string.IsNullOrWhiteSpace(campus.cam_codigo))
                {
                    return result.Error("Rol code is required");
                }

                var response = _campusRepository.CampusInsert(campus);

                if (response.CodeStatus == 1)
                {
                    return result.Ok(response);
                }
                else
                {
                    return result.Error(response);
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol inserting: {ex.Message}");
            }
        }
        public ServiceResult CampusUpdate(CampusDTO campus)
        {
            var result = new ServiceResult();

            try
            {
                if (campus == null)
                    return result.Error("Rol data is required");

                if (string.IsNullOrWhiteSpace(campus.cam_codigo))
                    return result.Error("Rol code is required for update");

                var response = _campusRepository.CampusUpdate(campus);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol updating: {ex.Message}");
            }
        }
        
        public ServiceResult CampusDelete(string campusCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(campusCodigo))
                return result.Error("Rol code is required for deletion");

            try
            {
                var response = _campusRepository.CampusDelete(campusCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during rol deletion: {ex.Message}");
            }
        }
    }
}
