using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ProjName.Common;
using ProjName.Application.Shared.Interfaces;

namespace ProjName.Infrastructure.Shared.Persistence;

public class EntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IRequestContext _requestContext;
    public EntitySaveChangesInterceptor(IDateTimeService dateTimeService, IRequestContext requestContext)
    {
        _dateTimeService = dateTimeService;
        _requestContext = requestContext;
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedById = _requestContext.GetLoggedInUserId();
                entry.Entity.CreatedAt = _dateTimeService.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedById = _requestContext.GetLoggedInUserId();
                entry.Entity.UpdatedAt = _dateTimeService.Now;
            }
        }
    }
}