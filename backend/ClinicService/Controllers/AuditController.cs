using ClinicService.Data;
using ClinicService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuditController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/audit/pacientes/123
    [HttpGet("pacientes/{id:int}")]
    public async Task<ActionResult<List<AuditLog>>> GetPacienteHistory(int id, CancellationToken ct)
    {
        var entityName = nameof(Paciente);
        var key = id.ToString();
        var logs = await _db.AuditLogs
            .AsNoTracking()
            .Where(a => a.Entity == entityName && a.EntityId == key)
            .OrderByDescending(a => a.ChangedAtUtc)
            .ToListAsync(ct);

        return Ok(logs);
    }

    // GET /api/audit/{entity}/{entityId}
    [HttpGet("{entity}/{entityId}")]
    public async Task<ActionResult<List<AuditLog>>> GetHistory(string entity, string entityId, CancellationToken ct)
    {
        var logs = await _db.AuditLogs
            .AsNoTracking()
            .Where(a => a.Entity == entity && a.EntityId == entityId)
            .OrderByDescending(a => a.ChangedAtUtc)
            .ToListAsync(ct);
        return Ok(logs);
    }
}
