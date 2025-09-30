using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAcademicGenerator.DataAccess
{
    public class ScriptDatabase
    {

        #region Classes

        public static string SP_CareersList = "[UNI].[SP_Careers_List]";
        public static string SP_CareerInsert = "[UNI].[SP_Career_Insert]";
        public static string SP_CareerUpdate = "[UNI].[SP_Career_Update]";
        public static string SP_CareerDelete = "[UNI].[SP_Career_Delete]";
        #endregion
        
        public static string SP_UsersList = "sp_Users_List";
        public static string SP_UserInsert = "sp_Users_Insert";
        public static string SP_UserUpdate = "sp_Users_Update";
        public static string SP_UserDelete = "sp_Users_Delete";
        
        public static string SP_UserRolesList = "sp_UserRoles_List";
        public static string SP_UserRoleInsert = "sp_UserRoles_Insert";
        public static string SP_UserRoleUpdate = "sp_UserRoles_Update";
        public static string SP_UserRoleDelete = "sp_UserRoles_Delete";
        
        public static string SP_RolList = "sp_Rol_List";
        public static string SP_RolInsert = "sp_Rol_Insert";
        public static string SP_RolUpdate = "sp_Rol_Update";
        public static string SP_RolDelete = "sp_Rol_Delete";

        #region Campus
        public const string SP_CampusList = "UNI.SP_Campus_List";
        public const string SP_CampusInsert = "UNI.SP_Campus_Insert";
        public const string SP_CampusUpdate = "UNI.SP_Campus_Update";
        public const string SP_CampusDelete = "UNI.SP_Campus_Delete";
        #endregion


        #region Modalities
        public static string SP_ModalitiesList = "[UNI].[SP_Modalities_List]";
        public static string SP_ModalityInsert = "[UNI].[SP_Modality_Insert]";
        public static string SP_ModalityUpdate = "[UNI].[SP_Modality_Update]";
        public static string SP_ModalityDelete = "[UNI].[SP_Modality_Delete]";
        #endregion

        #region Periods
        public static string SP_PeriodsList = "[UNI].[SP_Periods_List]";
        public static string SP_PeriodInsert = "[UNI].[SP_Period_Insert]";
        public static string SP_PeriodUpdate = "[UNI].[SP_Period_Update]";
        public static string SP_PeriodDelete = "[UNI].[SP_Period_Delete]";
        #endregion


        #region Areas
        public static string SP_AreasList  = "[ACA].[SP_Areas_List]";
        public static string SP_AreaInsert = "[ACA].[SP_Area_Insert]";
        public static string SP_AreaUpdate = "[ACA].[SP_Area_Update]";
        public static string SP_AreaDelete = "[ACA].[SP_Area_Delete]";
        #endregion
        
        #region Sections
        public static string SP_SectionsList  = "[ACA].[SP_Sections_List]";
        public static string SP_SectionInsert = "[ACA].[SP_Section_Insert]";
        public static string SP_SectionUpdate = "[ACA].[SP_Section_Update]";
        public static string SP_SectionDelete = "[ACA].[SP_Section_Delete]";
        #endregion

        #region Contacts
        public static string SP_ContactsList  = "[EXP].[SP_Contacts_List]";
        public static string SP_ContactInsert = "[EXP].[SP_Contact_Insert]";
        public static string SP_ContactUpdate = "[EXP].[SP_Contact_Update]";
        public static string SP_ContactDelete = "[EXP].[SP_Contact_Delete]";
        #endregion

        #region Teachers
        public static string SP_TeachersList  = "[ACA].[SP_Teachers_List]";
        public static string SP_TeacherInsert = "[ACA].[SP_Teacher_Insert]";
        public static string SP_TeacherUpdate = "[ACA].[SP_Teacher_Update]";
        public static string SP_TeacherDelete = "[ACA].[SP_Teacher_Delete]";
        #endregion

        #region Classroooms
        public static string SP_ClassroomsList  = "[ACA].[SP_Classrooms_List]";
        public static string SP_ClassroomInsert = "[ACA].[SP_Classroom_Insert]";
        public static string SP_ClassroomUpdate = "[ACA].[SP_Classroom_Update]";
        public static string SP_ClassromsDelete = "[ACA].[SP_Classroom_Delete]";
        #endregion

        #region Subjects
        public static string SP_SubjectsList  = "[ACA].[SP_Subjects_List]";
        public static string SP_SubjectInsert = "[ACA].[SP_Subject_Insert]";
        public static string SP_SubjectUpdate = "[ACA].[SP_Subject_Update]";
        public static string SP_SubjectDelete = "[ACA].[SP_Subject_Delete]";
        #endregion

        #region Students
        public const string SP_StudentsBulkInsert = "EXP.SP_Students_BulkInsert";
        public static string SP_StudentsList = "[EXP].[SP_Students_List]";
        public static string SP_StudentInsert = "[EXP].[SP_Student_Insert]";
        public static string SP_StudentUpdate = "[EXP].[SP_Student_Update]";
        public static string SP_StudentDelete = "[EXP].[SP_Student_Delete]";
        #endregion





    }
}
