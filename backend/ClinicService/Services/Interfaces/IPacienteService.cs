using ClinicService.Models;

namespace ClinicService.Services.Interfaces;

public interface IPacienteService
{
    Task<List<Paciente>> GetAllAsync(CancellationToken ct = default);
    Task<Paciente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Paciente> CreateAsync(Paciente entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, Paciente entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
