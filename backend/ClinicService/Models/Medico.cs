namespace ClinicService.Models;

public class Medico
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CRM { get; set; }
    public string? Especialidade { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
