using ClinicService.Models;

namespace ClinicService.Services.Interfaces;

public interface IMedicoService
{
    Task<List<Medico>> GetAllAsync(CancellationToken ct = default);
    Task<Medico?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Medico> CreateAsync(Medico entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, Medico entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
