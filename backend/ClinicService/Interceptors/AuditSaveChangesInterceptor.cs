using System.Text.Json;
using ClinicService.Data;
using ClinicService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClinicService.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context as AppDbContext;
        if (context is null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var http = _httpContextAccessor.HttpContext;
        var changedBy = http?.User?.Identity?.Name ?? "system";
        var requestId = http?.TraceIdentifier;
        var now = DateTime.UtcNow;

        var auditEntries = new List<AuditLog>();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog) continue; // don't audit audit
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted)) continue;

            var entityName = entry.Metadata.ClrType.Name;
            var key = GetPrimaryKeyValue(entry);

            var (oldVals, newVals, action) = BuildChangeSet(entry);

            // only log when there is something meaningful (added/deleted always meaningful)
            if (action == "Update" && oldVals.Count == 0 && newVals.Count == 0)
                continue;

            var log = new AuditLog
            {
                Entity = entityName,
                EntityId = key,
                Action = action,
                OldValues = oldVals.Count > 0 ? JsonSerializer.Serialize(oldVals) : null,
                NewValues = newVals.Count > 0 ? JsonSerializer.Serialize(newVals) : null,
                ChangedBy = changedBy,
                ChangedAtUtc = now,
                RequestId = requestId
            };
            auditEntries.Add(log);
        }

        if (auditEntries.Count > 0)
        {
            // Sanitize sensitive fields before persisting
            foreach (var entry in auditEntries)
            {
                entry.OldValues = SanitizeJson(entry.OldValues);
                entry.NewValues = SanitizeJson(entry.NewValues);
            }

            context.Set<AuditLog>().AddRange(auditEntries);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static string GetPrimaryKeyValue(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key == null) return string.Empty;
        var values = key.Properties.Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? string.Empty);
        return string.Join("|", values);
    }

    private static (Dictionary<string, object?> oldVals, Dictionary<string, object?> newVals, string action) BuildChangeSet(EntityEntry entry)
    {
        var oldVals = new Dictionary<string, object?>();
        var newVals = new Dictionary<string, object?>();
        string action = entry.State switch
        {
            EntityState.Added => "Insert",
            EntityState.Deleted => "Delete",
            _ => "Update"
        };

        foreach (var prop in entry.Properties)
        {
            if (prop.Metadata.IsPrimaryKey()) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    newVals[prop.Metadata.Name] = prop.CurrentValue;
                    break;
                case EntityState.Deleted:
                    oldVals[prop.Metadata.Name] = prop.OriginalValue;
                    break;
                case EntityState.Modified:
                    if (!prop.IsModified) continue;
                    oldVals[prop.Metadata.Name] = prop.OriginalValue;
                    newVals[prop.Metadata.Name] = prop.CurrentValue;
                    break;
            }
        }
        return (oldVals, newVals, action);
    }

    private static string? SanitizeJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return json;
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object?>>(json) ?? new();
            var sanitized = SanitizeDictionary(dict);
            return sanitized.Count > 0 ? JsonSerializer.Serialize(sanitized) : null;
        }
        catch
        {
            return json; // fallback
        }
    }

    private static Dictionary<string, object?> SanitizeDictionary(Dictionary<string, object?> dict)
    {
        var result = new Dictionary<string, object?>();
        foreach (var kv in dict)
        {
            var key = kv.Key;
            var value = kv.Value;
            if (IsSensitive(key))
            {
                result[key] = Mask(value?.ToString());
            }
            else
            {
                result[key] = value;
            }
        }
        return result;
    }

    private static bool IsSensitive(string key)
    {
        // Ajuste a lista conforme necessário
        return key.Equals("Documento", StringComparison.OrdinalIgnoreCase)
            || key.Equals("Email", StringComparison.OrdinalIgnoreCase)
            || key.Equals("Telefone", StringComparison.OrdinalIgnoreCase);
    }

    private static string Mask(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        // Máscara simples: mantém primeiros 2 e últimos 2 caracteres quando possível
        var s = input.Trim();
        if (s.Length <= 4) return new string('*', s.Length);
        return s.Substring(0, 2) + new string('*', s.Length - 4) + s.Substring(s.Length - 2, 2);
    }
}
