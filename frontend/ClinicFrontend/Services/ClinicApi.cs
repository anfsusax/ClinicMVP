using System.Net.Http.Json;
using ClinicFrontend.Models;

namespace ClinicFrontend.Services;

public class ClinicApi
{
    private readonly HttpClient _http;

    public ClinicApi(HttpClient http)
    {
        _http = http;
    }

    // Pacientes
    public async Task<List<PacienteReadDto>> GetPacientesAsync(string? nome = null, string? documento = null, CancellationToken ct = default)
    {
        var url = "api/Pacientes";
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(nome)) query.Add($"nome={Uri.EscapeDataString(nome)}");
        if (!string.IsNullOrWhiteSpace(documento)) query.Add($"documento={Uri.EscapeDataString(documento)}");
        if (query.Count > 0) url += "?" + string.Join("&", query);
        return await _http.GetFromJsonAsync<List<PacienteReadDto>>(url, ct) ?? new();
    }

    public async Task<PacienteReadDto?> GetPacienteAsync(int id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<PacienteReadDto>($"api/Pacientes/{id}", ct);

    public async Task<PacienteReadDto?> CreatePacienteAsync(PacienteCreateDto dto, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("api/Pacientes", dto, ct);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<PacienteReadDto>(cancellationToken: ct);
    }

    public async Task<bool> UpdatePacienteAsync(int id, PacienteUpdateDto dto, CancellationToken ct = default)
    {
        var res = await _http.PutAsJsonAsync($"api/Pacientes/{id}", dto, ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePacienteAsync(int id, CancellationToken ct = default)
    {
        var res = await _http.DeleteAsync($"api/Pacientes/{id}", ct);
        return res.IsSuccessStatusCode;
    }
}
