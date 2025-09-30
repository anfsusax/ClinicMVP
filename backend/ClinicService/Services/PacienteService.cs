using ClinicService.Data;
using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Services;

public class PacienteService : IPacienteService
{
    private readonly AppDbContext _db;

    public PacienteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Paciente> CreateAsync(Paciente entity, CancellationToken ct = default)
    {
        _db.Pacientes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Pacientes.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        _db.Pacientes.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<List<Paciente>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Pacientes.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Paciente?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Pacientes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> UpdateAsync(int id, Paciente entity, CancellationToken ct = default)
    {
        var existing = await _db.Pacientes.FindAsync(new object?[] { id }, ct);
        if (existing is null) return false;
        existing.Nome = entity.Nome;
        existing.Documento = entity.Documento;
        existing.DataNascimento = entity.DataNascimento;
        existing.Telefone = entity.Telefone;
        existing.Email = entity.Email;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
