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

        public static string SP_CareersList = "[UNI].[SP_CareersList]";
        public static string SP_CareerInsert = "[UNI].[SP_CareerInsert]";
        public static string SP_CareerUpdate = "[UNI].[SP_CareerUpdate]";
        public static string SP_CareerDelete = "[UNI].[SP_CareerDelete]";
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
        
        public const string SP_CampusList = "UNI.SP_CampusList";
        public const string SP_CampusInsert = "UNI.SP_CampusInsert";
        public const string SP_CampusUpdate = "UNI.SP_CampusUpdate";
        public const string SP_CampusDelete = "UNI.SP_CampusDelete";
    }
}
