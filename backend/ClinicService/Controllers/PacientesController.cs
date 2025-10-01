using ClinicService.DTOs;
using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _service;

    public PacientesController(IPacienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PacienteReadDto>>> GetAll(
        [FromQuery] string? nome,
        [FromQuery] string? documento,
        CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        if (!string.IsNullOrWhiteSpace(nome))
            items = items.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrWhiteSpace(documento))
            items = items.Where(p => string.Equals(p.Documento, documento, StringComparison.OrdinalIgnoreCase)).ToList();

        var result = items.Select(ToReadDto).ToList();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PacienteReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(ToReadDto(item));
    }

    [HttpPost]
    public async Task<ActionResult<PacienteReadDto>> Create([FromBody] PacienteCreateDto dto, CancellationToken ct)
    {
        var entity = new Paciente
        {
            Nome = dto.Nome,
            Documento = dto.Documento,
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            Email = dto.Email
        };
        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToReadDto(created));
    }

    [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] PacienteUpdateDto dto, CancellationToken ct)
    {
        var entity = new Paciente
        {
            Nome = dto.Nome,
            Documento = dto.Documento,
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            Email = dto.Email
        };
        var ok = await _service.UpdateAsync(id, entity, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    private static PacienteReadDto ToReadDto(Paciente p) => new(
        p.Id,
        p.Nome,
        p.Documento,
        p.DataNascimento,
        p.Telefone,
        p.Email
    );
}
