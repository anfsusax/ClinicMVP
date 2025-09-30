using ClinicService.Models;

namespace ClinicService.Services.Interfaces;

public interface IConsultaService
{
    Task<List<Consulta>> GetAllAsync(CancellationToken ct = default);
    Task<Consulta?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Consulta> CreateAsync(Consulta entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, Consulta entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
