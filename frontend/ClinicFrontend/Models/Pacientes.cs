namespace ClinicFrontend.Models;

public record PacienteReadDto(int Id, string Nome, string? Documento, DateOnly? DataNascimento, string? Telefone, string? Email);
public record PacienteCreateDto(string Nome, string? Documento, DateOnly? DataNascimento, string? Telefone, string? Email);
public record PacienteUpdateDto(string? Nome, string? Documento, DateOnly? DataNascimento, string? Telefone, string? Email);
