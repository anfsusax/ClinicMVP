using ClinicService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Paciente>(e =>
        {
            e.Property(p => p.Nome).HasMaxLength(200).IsRequired();
            e.Property(p => p.Documento).HasMaxLength(50);
            e.Property(p => p.Email).HasMaxLength(200);
            e.Property(p => p.Telefone).HasMaxLength(50);

            // Soft delete filter
            e.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Medico>(e =>
        {
            e.Property(m => m.Nome).HasMaxLength(200).IsRequired();
            e.Property(m => m.CRM).HasMaxLength(50);
            e.Property(m => m.Especialidade).HasMaxLength(100);
        });

        modelBuilder.Entity<Consulta>(e =>
        {
            e.HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.Medico)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.Property(a => a.Entity).HasMaxLength(200).IsRequired();
            e.Property(a => a.EntityId).HasMaxLength(200).IsRequired();
            e.Property(a => a.Action).HasMaxLength(20).IsRequired();
            e.Property(a => a.ChangedBy).HasMaxLength(200);
            e.Property(a => a.RequestId).HasMaxLength(100);
        });
    }
}
