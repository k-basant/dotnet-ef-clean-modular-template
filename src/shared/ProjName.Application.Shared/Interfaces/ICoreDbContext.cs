using ProjName.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ProjName.Application.Shared.Interfaces;
public interface ICoreDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<T> GetDbSet<T, TPK>() where T : BaseEntity<TPK> where TPK: struct;
    string ConnectionString { get; }

}

