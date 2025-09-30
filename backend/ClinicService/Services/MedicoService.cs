using ClinicService.Data;
using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Services;

public class MedicoService : IMedicoService
{
    private readonly AppDbContext _db;

    public MedicoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Medico> CreateAsync(Medico entity, CancellationToken ct = default)
    {
        _db.Medicos.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Medicos.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        _db.Medicos.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<List<Medico>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Medicos.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Medico?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Medicos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(int id, Medico entity, CancellationToken ct = default)
    {
        var existing = await _db.Medicos.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        existing.Nome = entity.Nome;
        existing.CRM = entity.CRM;
        existing.Especialidade = entity.Especialidade;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
