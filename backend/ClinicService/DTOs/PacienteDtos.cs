using System.ComponentModel.DataAnnotations;

namespace ClinicService.DTOs;

public record PacienteCreateDto(
    [Required]
    [StringLength(200)]
    string Nome,

    [StringLength(50)]
    string? Documento,

    DateOnly? DataNascimento,

    [StringLength(50)]
    string? Telefone,

    [EmailAddress]
    [StringLength(200)]
    string? Email
);

// Campos opcionais para permitir atualização parcial (não sobrescreve com null)
public record PacienteUpdateDto(
    [StringLength(200)]
    string? Nome,

    [StringLength(50)]
    string? Documento,

    DateOnly? DataNascimento,

    [StringLength(50)]
    string? Telefone,

    [EmailAddress]
    [StringLength(200)]
    string? Email
);

public record PacienteReadDto(
    int Id,
    string Nome,
    string? Documento,
    DateOnly? DataNascimento,
    string? Telefone,
    string? Email
);
