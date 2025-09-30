using ClinicService.Data;
using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Services;

public class ConsultaService : IConsultaService
{
    private readonly AppDbContext _db;

    public ConsultaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Consulta> CreateAsync(Consulta entity, CancellationToken ct = default)
    {
        _db.Consultas.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Consultas.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        _db.Consultas.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<List<Consulta>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .ToListAsync(ct);
    }

    public async Task<Consulta?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(int id, Consulta entity, CancellationToken ct = default)
    {
        var existing = await _db.Consultas.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        existing.DataHora = entity.DataHora;
        existing.Observacoes = entity.Observacoes;
        existing.PacienteId = entity.PacienteId;
        existing.MedicoId = entity.MedicoId;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
