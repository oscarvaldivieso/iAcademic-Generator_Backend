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
        private readonly StudentsRepository _studentsRepository;

        public EXPServices(ContactsRepository contactsRepository, StudentsRepository studentsRepository )
        {
         
            _contactsRepository = contactsRepository;
            _studentsRepository = studentsRepository;
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

        #region Students

        /// <summary>
        /// Inserta o actualiza múltiples estudiantes desde Excel
        /// </summary>
        public ServiceResult StudentsBulkInsert(BulkInsertRequestDTO request, string createdBy)
        {
            var result = new ServiceResult();

            try
            {
                // Validaciones básicas
                if (request?.Estudiantes == null || !request.Estudiantes.Any())
                {
                    return result.BadRequest("No se proporcionaron estudiantes para insertar");
                }

                // Validar que no haya códigos duplicados en la petición
                var duplicados = request.Estudiantes
                    .GroupBy(e => e.est_codigo)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicados.Any())
                {
                    return result.BadRequest($"Códigos duplicados en la petición: {string.Join(", ", duplicados)}");
                }

                // Validar datos requeridos
                var erroresValidacion = ValidarEstudiantes(request.Estudiantes);
                if (erroresValidacion.Any())
                {
                    return result.BadRequest($"Errores de validación: {string.Join("; ", erroresValidacion)}");
                }

                // Ejecutar bulk insert
                var response = _studentsRepository.Students_BulkInsert(request, createdBy);

                // Evaluar respuesta
                if (response.Success)
                {
                    return result.Ok(response);
                }
                else if (response.ErroresEncontrados > 0 &&
                        (response.EstudiantesInsertados > 0 || response.EstudiantesActualizados > 0))
                {
                    // Procesamiento parcial: algunos exitosos, algunos con errores
                    return result.Ok("Procesamiento completado con algunos errores", response);
                }
                else
                {
                    // Todos fallaron
                    result.Data = response;
                    return result.Error(response.Message);
                }
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado durante bulk insert: {ex.Message}");
            }
        }

        /// <summary>
        /// Valida datos de estudiantes sin insertar (dry-run)
        /// </summary>
        public ServiceResult ValidateStudentsBulk(BulkInsertRequestDTO request)
        {
            var result = new ServiceResult();

            try
            {
                if (request?.Estudiantes == null || !request.Estudiantes.Any())
                {
                    return result.BadRequest("No se proporcionaron estudiantes para validar");
                }

                var errores = ValidarEstudiantes(request.Estudiantes);

                if (errores.Any())
                {
                    result.Data = new { errores };
                    return result.BadRequest("Errores de validación encontrados");
                }

                return result.Ok(new
                {
                    message = "Validación exitosa",
                    totalRegistros = request.Estudiantes.Count,
                    carrerasUnicas = request.Estudiantes.Select(e => e.car_codigo).Distinct().Count(),
                    campusUnicos = request.Estudiantes.Select(e => e.cam_codigo).Distinct().Count(c => !string.IsNullOrEmpty(c))
                });
            }
            catch (Exception ex)
            {
                return result.Error($"Error durante validación: {ex.Message}");
            }
        }

        /// <summary>
        /// Método privado para validar estructura de datos
        /// </summary>
        private List<string> ValidarEstudiantes(List<StudentBulkDTO> estudiantes)
        {
            var errores = new List<string>();

            for (int i = 0; i < estudiantes.Count; i++)
            {
                var estudiante = estudiantes[i];
                var fila = i + 1; // Para referencia del usuario (Excel row)

                // Validar código de estudiante
                if (string.IsNullOrWhiteSpace(estudiante.est_codigo))
                {
                    errores.Add($"Fila {fila}: Código de estudiante es requerido");
                }
                else if (estudiante.est_codigo.Length > 20)
                {
                    errores.Add($"Fila {fila}: Código de estudiante muy largo (máx 20 caracteres)");
                }

                // Validar nombre
                if (string.IsNullOrWhiteSpace(estudiante.est_nombre))
                {
                    errores.Add($"Fila {fila} ({estudiante.est_codigo}): Nombre de estudiante es requerido");
                }
                else if (estudiante.est_nombre.Length > 150)
                {
                    errores.Add($"Fila {fila} ({estudiante.est_codigo}): Nombre muy largo (máx 150 caracteres)");
                }

                // Validar código de carrera
                if (string.IsNullOrWhiteSpace(estudiante.car_codigo))
                {
                    errores.Add($"Fila {fila} ({estudiante.est_codigo}): Código de carrera es requerido");
                }

                // Validar género
                if (!string.IsNullOrEmpty(estudiante.est_genero))
                {
                    var generoUpper = estudiante.est_genero.ToUpper();
                    if (generoUpper != "M" && generoUpper != "F" && generoUpper != "MASCULINO" && generoUpper != "FEMENINO")
                    {
                        errores.Add($"Fila {fila} ({estudiante.est_codigo}): Género inválido. Use M, F, Masculino o Femenino");
                    }
                }

                // Validar índices
                if (estudiante.est_indice_general.HasValue)
                {
                    if (estudiante.est_indice_general < 0 || estudiante.est_indice_general > 100)
                    {
                        errores.Add($"Fila {fila} ({estudiante.est_codigo}): Índice general debe estar entre 0 y 100");
                    }
                }

                if (estudiante.est_indice_graduacion.HasValue)
                {
                    if (estudiante.est_indice_graduacion < 0 || estudiante.est_indice_graduacion > 100)
                    {
                        errores.Add($"Fila {fila} ({estudiante.est_codigo}): Índice de graduación debe estar entre 0 y 100");
                    }
                }

                // Validar año de plan si se proporciona
                if (estudiante.car_anio_plan.HasValue)
                {
                    var anioActual = DateTime.Now.Year;
                    if (estudiante.car_anio_plan < 1900 || estudiante.car_anio_plan > anioActual + 5)
                    {
                        errores.Add($"Fila {fila} ({estudiante.est_codigo}): Año de plan inválido");
                    }
                }
            }

            return errores;
        }

        #endregion



    }
}
