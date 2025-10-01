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
        // Soft delete
        existing.IsDeleted = true;
        existing.DeletedAt = DateTime.UtcNow;
        existing.DeletedBy = "system"; // substituir por usuário autenticado quando houver
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

        // Atualização parcial: só sobrescreve quando o valor foi informado
        if (!string.IsNullOrWhiteSpace(entity.Nome))
            existing.Nome = entity.Nome;

        if (entity.Documento is not null)
            existing.Documento = entity.Documento;

        if (entity.DataNascimento.HasValue)
            existing.DataNascimento = entity.DataNascimento;

        if (entity.Telefone is not null)
            existing.Telefone = entity.Telefone;

        if (entity.Email is not null)
            existing.Email = entity.Email;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
