using DevEventsApi.Entities;
using DevEventsApi.Entities.Interfaces;
using DevEventsApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevEventsApi.Persistence.Interceptors;

public class AuditableEntitiesInterceptor : SaveChangesInterceptor
{
  private IHttpContextAccessor httpContextAccessor;

  private readonly string username;

  private readonly Dictionary<EntityState, string> entityStatesMap;

  private readonly List<EntityState> allowedEntityStatesForTracking;

  public AuditableEntitiesInterceptor(IHttpContextAccessor httpContextAccessor)
  {
    this.httpContextAccessor = httpContextAccessor;
    username = AuthorizeHelper.GetUsernameOrRemoteAddress(httpContextAccessor.HttpContext);
    entityStatesMap = new() { { EntityState.Added, "INSERT" }, { EntityState.Deleted, "DELETE" }, { EntityState.Modified, "UPDATE" } };
    allowedEntityStatesForTracking = new() { EntityState.Added, EntityState.Deleted, EntityState.Modified };
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
  {
    var context = eventData.Context;
    if (context is null)
      return base.SavingChangesAsync(eventData, result, cancellationToken);

    var auditingEntries = OnBeforeSavingChanges(context);

    context.AddRange(auditingEntries);

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private List<Auditing> OnBeforeSavingChanges(DbContext context)
  {
    var auditingEntries = new List<Auditing>();

    var auditableEntries = context.ChangeTracker.Entries<IAuditable>();

    foreach (var entityEntry in auditableEntries)
    {
      if (allowedEntityStatesForTracking.Contains(entityEntry.State))
      {
        if (entityEntry.State.Equals(EntityState.Added))
          entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;

        if (entityEntry.State.Equals(EntityState.Modified))
          entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;

        var auditingEntry = new Auditing
        {
          EntityName = entityEntry.Metadata.ClrType.Name,
          ActionType = entityStatesMap[entityEntry.State],
          EntityUid = entityEntry.Properties.Single(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? $"{entityEntry.Metadata.ClrType.Name} - Primary Key not found.",
          TimeStamp = DateTime.UtcNow,
          Username = username,
          Changes = entityEntry.Properties.Select(p => new { p.Metadata.Name, p.CurrentValue }).ToDictionary(i => i.Name, i => i.CurrentValue),
        };

        auditingEntries.Add(auditingEntry);
      }
    }

    return auditingEntries;
  }
}