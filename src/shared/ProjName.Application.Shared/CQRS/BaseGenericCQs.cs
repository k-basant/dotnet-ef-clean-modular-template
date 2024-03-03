using ProjName.Application.Shared.Interfaces;

namespace ProjName.Application.Shared.CQRS;

// Types defined here are suggested to be used by the shared projects only. For native development please use types defined in `BaseCQs.cs`

#region Get Single
public abstract class GetSingleQuery<TEntity, TVM, TPK> : IRequest<SingleVM<TVM, TPK>>
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
    where TPK: struct
{
    public TPK Id { get; set; }
    public bool IsDetailed { get; set; }
}
public abstract class GetSingleQueryHandler<TEntity, TPK, TVM, TDetailVM, TDbContext, TCommand> : BaseHandler<TDbContext>, IRequestHandler<TCommand, SingleVM<TVM, TPK>>
    where TCommand : GetSingleQuery<TEntity, TVM, TPK>
    where TEntity : BaseEntity<TPK>
    where TPK : struct
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
    where TDetailVM : TVM
    where TDbContext : ICoreDbContext
{
    public GetSingleQueryHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }

    public virtual async Task<SingleVM<TVM, TPK>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.GetDbSet<TEntity, TPK>().AsQueryable();

        if (request.IsDetailed)
        {
            query = query.BuildIncludeExp(typeof(TDetailVM));
        }
        var data = await query.FirstOrDefaultAsync(x => x.Id.Equals(request.Id) && !x.IsDeleted);
        return new SingleVM<TVM, TPK>
        {
            Data = request.IsDetailed ? _mapper.Map<TDetailVM>(data) : _mapper.Map<TVM>(data),
            Status = data == null ? System.Net.HttpStatusCode.NotFound : System.Net.HttpStatusCode.OK
        };
    }
}
#endregion

#region Get List
public abstract class BaseGetListQuery<TVM, TPK> : IRequest<ListVM<TVM, TPK>>, IBaseGetListQuery
    where TVM : BaseEntityVM<TPK>
    where TPK : struct
{
    public GetAllQueryOptions Options { get; set; } = new GetAllQueryOptions();
}
public abstract class GetListQuery<TEntity, TPK, TVM> : BaseGetListQuery<TVM, TPK>
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
    where TPK : struct
{
    public FilterExp<TPK> Id { get; set; }
    public FilterExp<TPK?> CreatedById { get; set; }
    public FilterExp<TPK?> UpdatedById { get; set; }
    public FilterExp<DateTime> CreatedAt { get; set; }
    public FilterExp<DateTime?> UpdatedAt { get; set; }
}
public abstract class GetListQueryHandler<TEntity, TPK, TVM, TDetailVM, TDbContext, TQuery> : BaseHandler<TDbContext>, IRequestHandler<TQuery, ListVM<TVM, TPK>>
    where TQuery : BaseGetListQuery<TVM, TPK>
    where TEntity : BaseEntity<TPK>
    where TPK: struct
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
    where TDetailVM : TVM
    where TDbContext : ICoreDbContext
{
    public GetListQueryHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }
    public virtual async Task<ListVM<TVM, TPK>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = await BuildQueryExpression(request);
        query = query.Where(x => !x.IsDeleted);

        var pagedResult = query.BuildPagingExp(request.Options.PageNum, request.Options.PageSize);
        query = pagedResult.Query;

        var data = await query.ToListAsync();
        return new ListVM<TVM, TPK>
        {
            Data = !request.Options.IsDetailed ? _mapper.Map<List<TVM>>(data) : (dynamic)_mapper.Map<List<TDetailVM>>(data),
            PageInfo = new PageDetails(request.Options.PageNum, request.Options.PageSize, pagedResult.TotalCount)
        };
    }
    protected virtual Task<IQueryable<TEntity>> BuildQueryExpression(TQuery request)
    {
        var query = _dbContext.GetDbSet<TEntity, TPK>().AsQueryable();
        query = query.BuildFilterExpression(request);
        if (request.Options.IsDetailed)
        {
            query = query.BuildIncludeExp(typeof(TDetailVM));
        }
        if (request.Options.SortBy != null && request.Options.SortBy != "")
        {
            query = query.BuildSortingExp(request.Options.SortBy, request.Options.SortDescending);
        }

        return Task.FromResult(query);
    }
}
#endregion

#region Save
public abstract class SaveSingleCommand<TEntity, TPK, TVM> : BaseFileIM, IRequest<SingleVM<TVM, TPK>>, IMapTo<TEntity>
    where TEntity : BaseEntity<TPK>
    where TPK : struct
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
{
    /// <summary>
    /// When Id is provided, the command will be considered for adding the entity.
    /// </summary>
    public TPK? Id { get; set; }
}
public abstract class SaveSingleCommandHandler<TEntity, TPK, TVM, TDbContext, TCommand> : BaseHandler<TDbContext>, IRequestHandler<TCommand, SingleVM<TVM, TPK>>
    where TCommand : SaveSingleCommand<TEntity, TPK, TVM>
    where TEntity : BaseEntity<TPK>
    where TPK : struct
    where TVM : BaseEntityVM<TPK>, IMapFrom<TEntity>
    where TDbContext : ICoreDbContext
{
    public SaveSingleCommandHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }

    public virtual async Task<SingleVM<TVM, TPK>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        TEntity existing;
        if (!request.Id.HasValue)
        {
            existing = _mapper.Map<TEntity>(request);
            await ActForAdd(request, existing);

            _dbContext.GetDbSet<TEntity, TPK>().Add(existing);
        }
        else
        {
            existing = _dbContext.GetDbSet<TEntity, TPK>().FirstOrDefault(x => x.Id.Equals(request.Id) && x.IsDeleted == false);
            if (existing == null)
            {
                throw new FluentValidation.ValidationException("No record found with the given Id.");
            }

            await ActForUpdate(request, existing);
            existing = _mapper.Map(request, existing);
        }

        await ActBeforeSave(request, existing);

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        return new SingleVM<TVM, TPK> { Data = _mapper.Map<TVM>(existing) };
    }
    protected virtual Task ActForAdd(TCommand request, TEntity entityToBeAdded)
    {
        return Task.CompletedTask;
    }
    protected virtual Task ActForUpdate(TCommand request, TEntity entityToBeUpdated)
    {
        return Task.CompletedTask;
    }
    protected virtual Task ActBeforeSave(TCommand request, TEntity entityToBeSaved)
    {
        return Task.CompletedTask;
    }
}
#endregion

#region Delete
public abstract class DeleteSingleCommand<TEntity, TPK> : IRequest<BaseVM>
    where TEntity : BaseEntity<TPK>
    where TPK : struct
{
    public TPK Id { get; set; }
}
public abstract class DeleteSingleCommandHandler<TEntity, TPK, TDbContext, TCommand> : IRequestHandler<TCommand, BaseVM>
    where TCommand : DeleteSingleCommand<TEntity, TPK>
    where TEntity : BaseEntity<TPK>
    where TPK : struct
    where TDbContext : ICoreDbContext
{
    protected readonly TDbContext _dbContext;
    protected DeleteSingleCommandHandler(TDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<BaseVM> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var existing = (await FetchQueryExpression(request)).FirstOrDefault();

        if (existing == null)
        {
            throw new FluentValidation.ValidationException("No record found with the given Id.");
        }

        if (!existing.IsDeleted)
        {
            existing.IsDeleted = true;
            existing.DeletedAt = DateTime.UtcNow;
            await ActForDelete(request, existing);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }
        return new BaseVM("Deleted successfully!");
    }
    protected virtual Task ActForDelete(TCommand request, TEntity entityToBeDeleted)
    {
        return Task.CompletedTask;
    }
    protected virtual Task<IQueryable<TEntity>> FetchQueryExpression(TCommand request)
    {
        var query = _dbContext.GetDbSet<TEntity, TPK>().Where(x => x.Id.Equals(request.Id) && x.IsDeleted == false);
        return Task.FromResult(query);
    }
}
#endregion