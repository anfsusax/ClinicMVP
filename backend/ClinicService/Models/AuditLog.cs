using System.Text.Json;

namespace ClinicService.Models;

public class AuditLog
{
    public long Id { get; set; }
    public string Entity { get; set; } = string.Empty;          // e.g., "Paciente"
    public string EntityId { get; set; } = string.Empty;         // string para generalizar PKs
    public string Action { get; set; } = string.Empty;           // Insert | Update | Delete
    public string? OldValues { get; set; }                       // JSON
    public string? NewValues { get; set; }                       // JSON
    public string? ChangedBy { get; set; }
    public DateTime ChangedAtUtc { get; set; }
    public string? RequestId { get; set; }
}
