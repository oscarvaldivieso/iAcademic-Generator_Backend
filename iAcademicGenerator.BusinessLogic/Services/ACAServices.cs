using iAcademicGenerator.DataAccess.Repositories.ACA;
using iAcademicGenerator.DataAccess.Repositories.UNI;
using iAcademicGenerator.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iAcademicGenerator.DataAccess.Repositories.ACA.RequestsRepository;

namespace iAcademicGenerator.BusinessLogic.Services
{
    public class ACAServices
    {
        private readonly SectionsRepository _sectionsRepository;
        private readonly ClassroomsRepository _classroomsRepository;
        private readonly TeachersRepository _teachersRepository;
  
        private readonly SubjectsRepository _subjectsRepository;
        private readonly AreasRepository _areasRepository;
        private readonly RequestsRepository _requestsRepository;
        private readonly SchedulesRepository _schedulesRepository;

        public ACAServices(SectionsRepository sectionsRepository, ClassroomsRepository classroomsRepository,
            TeachersRepository teachersRepository,  
            SubjectsRepository subjectsRepository, AreasRepository areasRepository, RequestsRepository requestsRepository, SchedulesRepository schedulesRepository )
        {
            _sectionsRepository = sectionsRepository;
            _classroomsRepository = classroomsRepository;
            _teachersRepository = teachersRepository;
            _subjectsRepository = subjectsRepository;
            _areasRepository = areasRepository;
            _requestsRepository = requestsRepository;
            _schedulesRepository = schedulesRepository;

        }


        #region Sections
        public ServiceResult ListSections()
        {
            var result = new ServiceResult();
            try
            {
                var response = _sectionsRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult SectionsInsert(SectionsDTO sections)
        {
            var result = new ServiceResult();

            try
            {
                // Validaciones básicas antes de llamar al repository
                if (sections == null)
                {
                    return result.Error("Section data is required");
                }

                if (string.IsNullOrWhiteSpace(sections.sec_codigo))
                {
                    return result.Error("Section code is required");
                }

                var response = _sectionsRepository.SectionInsert(sections);

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
                return result.Error($"Unexpected error during Section inserting: {ex.Message}");
            }
        }
        public ServiceResult SectionsUpdate(SectionsDTO sections)
        {
            var result = new ServiceResult();

            try
            {
                if (sections == null)
                    return result.Error("Section data is required");

                if (string.IsNullOrWhiteSpace(sections.sec_codigo))
                    return result.Error("Section code is required for update");

                var response = _sectionsRepository.SectionUpdate(sections);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Section updating: {ex.Message}");
            }
        }
        public ServiceResult SectionsDelete(string sec_codigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(sec_codigo))
                return result.Error("Section code is required for deletion");

            try
            {
                var response = _sectionsRepository.SectionDelete(sec_codigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Section deletion: {ex.Message}");
            }
        }
        #endregion

        #region Classrooms
        public ServiceResult ListClassrooms()
        {
            var result = new ServiceResult();
            try
            {
                var response = _classroomsRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult ClassroomsInsert(ClassroomsDTO classroom)
        {
            var result = new ServiceResult();

            try
            {
                // Validaciones básicas antes de llamar al repository
                if (classroom == null)
                {
                    return result.Error("Classroom data is required");
                }

                if (string.IsNullOrWhiteSpace(classroom.auc_codigo))
                {
                    return result.Error("Classroom code is required");
                }

                var response = _classroomsRepository.ClassroomInsert(classroom);

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
                return result.Error($"Unexpected error during Classroom inserting: {ex.Message}");
            }
        }
        public ServiceResult ClassroomsUpdate(ClassroomsDTO classrooms)
        {
            var result = new ServiceResult();

            try
            {
                if (classrooms == null)
                    return result.Error("Classroom data is required");

                if (string.IsNullOrWhiteSpace(classrooms.auc_codigo))
                    return result.Error("Classroom code is required for update");

                var response = _classroomsRepository.ClassroomUpdate(classrooms);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Classroom updating: {ex.Message}");
            }
        }
         public ServiceResult ClassroomsDelete(string classCodigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(classCodigo))
                return result.Error("Classroom code is required for deletion");

            try
            {
                var response = _classroomsRepository.ClassroomDelete(classCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Classroom deletion: {ex.Message}");
            }
        }
        #endregion

        #region Teachers
        public ServiceResult ListTeachers()
        {
            var result = new ServiceResult();
            try
            {
                var response = _teachersRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult TeachersInsert(TeachersDTO teachers)
        {
            var result = new ServiceResult();

            try
            {
              
                var response = _teachersRepository.TeacherInsert(teachers);

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
                return result.Error($"Unexpected error during Teacher inserting: {ex.Message}");
            }
        }
        public ServiceResult TeachersUpdate(TeachersDTO teachers)
        {
            var result = new ServiceResult();

            try
            {
             
                var response = _teachersRepository.TeacherUpdate(teachers);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Teacher updating: {ex.Message}");
            }
        }
        public ServiceResult TeachersDelete(int id)
        {
            var result = new ServiceResult();

            if (id == null)
                return result.Error("Teacher code is required for deletion");

            try
            {
                var response = _teachersRepository.TeacherDelete(id);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Teacher deletion: {ex.Message}");
            }
        }
        #endregion

        #region Subjects
        public ServiceResult ListSubjects()
        {
            var result = new ServiceResult();
            try
            {
                var response = _subjectsRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult SubjectsInsert(SubjectDTO subjects)
        {
            var result = new ServiceResult();

            try
            {

                var response = _subjectsRepository.SubjectInsert(subjects);

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
                return result.Error($"Unexpected error during Subject inserting: {ex.Message}");
            }
        }
        public ServiceResult SubjectsUpdate(SubjectDTO subjects)
        {
            var result = new ServiceResult();

            try
            {

                var response = _subjectsRepository.SubjectUpdate(subjects);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Subject updating: {ex.Message}");
            }
        }
        public ServiceResult SubjectsDelete(string id)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(id))
                return result.Error("Subject code is required for deletion");

            try
            {
                var response = _subjectsRepository.SubjectDelete(id);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Subject deletion: {ex.Message}");
            }
        }
        #endregion


        #region Areas
        public ServiceResult ListAreas()
        {
            var result = new ServiceResult();
            try
            {
                var response = _areasRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }
        public ServiceResult AreasInsert(AreasDTO areas)
        {
            var result = new ServiceResult();

            try
            {

                var response = _areasRepository.AreaInsert(areas);

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
                return result.Error($"Unexpected error during Area inserting: {ex.Message}");
            }
        }
        public ServiceResult AreasUpdate(AreasDTO areas)
        {
            var result = new ServiceResult();

            try
            {

                var response = _areasRepository.AreaUpdate(areas);

                if (response.CodeStatus == 1)
                    return result.Ok(response);
                else
                    return result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Area updating: {ex.Message}");
            }
        }
        public ServiceResult AreasDelete(string areCode)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(areCode))
                return result.Error("Area code is required for deletion");

            try
            {
                var response = _areasRepository.AreaDelete(areCode);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Area deletion: {ex.Message}");
            }
        }
        #endregion

        #region Requests
        public ServiceResult RequestAssignmentInsert(RequestAssignmentDTO request)
        {
            var result = new ServiceResult();
            try
            {
                // Validaciones básicas
                if (request == null)
                {
                    return result.Error("Request data is required");
                }

                if (string.IsNullOrWhiteSpace(request.pre_codest))
                {
                    return result.Error("Student code is required");
                }

                if (string.IsNullOrWhiteSpace(request.created_by))
                {
                    return result.Error("Created by is required");
                }

                if (request.Materias == null || !request.Materias.Any())
                {
                    return result.Error("At least one subject is required");
                }

                // Validar cada materia
                foreach (var materia in request.Materias)
                {
                    if (string.IsNullOrWhiteSpace(materia.mat_codigo))
                    {
                        return result.Error("Subject code is required for all subjects");
                    }

                    if (int.IsNegative(materia.hor_codigo))
                    {
                        return result.Error("Section code is required for all subjects");
                    }

                }

                var response = _requestsRepository.RequestAssignmentInsert(request);

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
                return result.Error($"Unexpected error during Request assignment inserting: {ex.Message}");
            }
        }

        public ServiceResult RequestsStudentList(string est_codigo)
        {
            var result = new ServiceResult();

            if (string.IsNullOrWhiteSpace(est_codigo))
                return result.Error("Student code is required.");

            try
            {
                var response = _requestsRepository.List(est_codigo);
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error listing student requests: {ex.Message}");
            }
        }


        #endregion

        #region Schedules
        public ServiceResult ListSchedules()
        {
            var result = new ServiceResult();
            try
            {
                var response = _schedulesRepository.List();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error(ex.Message);
            }
        }

        public ServiceResult ScheduleInsert(SchedulesDTO schedule)
        {
            var result = new ServiceResult();
            try
            {
                var response = _schedulesRepository.ScheduleInsert(schedule);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Schedule insert: {ex.Message}");
            }
        }

        public ServiceResult ScheduleUpdate(SchedulesDTO schedule)
        {
            var result = new ServiceResult();
            try
            {
                var response = _schedulesRepository.ScheduleUpdate(schedule);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Schedule update: {ex.Message}");
            }
        }

        public ServiceResult ScheduleDelete(int horCodigo)
        {
            var result = new ServiceResult();

            if (horCodigo <= 0)
                return result.Error("Schedule code is required for deletion");

            try
            {
                var response = _schedulesRepository.ScheduleDelete(horCodigo);

                return response.CodeStatus == 1
                    ? result.Ok(response)
                    : result.Error(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Unexpected error during Schedule deletion: {ex.Message}");
            }
        }
        #endregion


    }
}
