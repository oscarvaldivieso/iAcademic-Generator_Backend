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