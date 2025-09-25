namespace iAcademicGenerator.Models.Models;

public class UserDTO
{
    public string? usu_codigo { get; set; }
    public string  usu_nombre { get; set; }
    public string usu_password { get; set; }
    public string usu_email { get; set; }
    public bool usu_activo { get; set; }
    public DateTime usu_ultimo_login { get; set; }
    public int usu_intentos_fallidos { get; set; }
    public bool usu_bloqueado { get; set; }
    public int active { get; set; }
    public DateTime? create_at { get; set; }
    public DateTime? update_at { get; set; }
    public string? create_by { get; set; }
    public string? update_by { get; set; }
    
    
}