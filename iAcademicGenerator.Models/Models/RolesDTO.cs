namespace iAcademicGenerator.Models.Models;

public class RolesDTO
{
    public string rol_codigo { get; set; }
    public string rol_nombre { get; set; }
    public bool active { get; set; }
    public DateTime create_at { get; set; }
    public DateTime update_at { get; set; }
    public string create_by  { get; set; }
    public string update_by { get; set; }
}


public class RoleCreateDTO
{
    public string rol_codigo { get; set; }
    public string rol_nombre { get; set; }
    public List<string> permisos { get; set; } // Lista de códigos de permisos
    public string created_by { get; set; }
}

// RoleUpdateDTO.cs
public class RoleUpdateDTO
{
    public string rol_codigo { get; set; }
    public string rol_nombre { get; set; }
    public List<string> permisos { get; set; }
    public string updated_by { get; set; }
}

// PermissionWithStatusDTO.cs
public class PermissionWithStatusDTO
{
    public string perm_codigo { get; set; }
    public string perm_nombre { get; set; }
    public string perm_descripcion { get; set; }
    public bool asignado { get; set; }
}