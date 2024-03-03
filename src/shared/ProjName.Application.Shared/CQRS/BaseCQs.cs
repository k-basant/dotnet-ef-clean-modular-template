using ProjName.Application.Shared.Interfaces;

namespace ProjName.Application.Shared.CQRS;

#region Get Single
public abstract class GetSingleQuery<TEntity, TVM> : GetSingleQuery<TEntity, TVM, Guid>
    where TVM: BaseEntityVM, IMapFrom<TEntity>
{

}
public abstract class GetSingleQueryHandler<TEntity, TVM, TDetailVM, TDbContext, TCommand> : GetSingleQueryHandler<TEntity, Guid, TVM, TDetailVM, TDbContext, TCommand>
    where TCommand : GetSingleQuery<TEntity, TVM>
    where TEntity : BaseEntity
    where TVM : BaseEntityVM, IMapFrom<TEntity>
    where TDetailVM : TVM
    where TDbContext : ICoreDbContext
{
    public GetSingleQueryHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }
}
#endregion

#region Get List
public interface IBaseGetListQuery
{
    GetAllQueryOptions Options { get; set; }
}
public abstract class BaseGetListQuery<TVM> : BaseGetListQuery<TVM, Guid>
    where TVM : BaseEntityVM
{
}
public abstract class GetListQuery<TEntity, TVM> : GetListQuery<TEntity, Guid, TVM>
    where TVM : BaseEntityVM, IMapFrom<TEntity>
{

}
public abstract class GetListQueryHandler<TEntity, TVM, TDetailVM, TDbContext, TQuery> : GetListQueryHandler<TEntity, Guid, TVM, TDetailVM, TDbContext, TQuery>
    where TQuery : BaseGetListQuery<TVM, Guid>
    where TEntity : BaseEntity
    where TVM : BaseEntityVM, IMapFrom<TEntity>
    where TDetailVM : TVM
    where TDbContext : ICoreDbContext
{
    public GetListQueryHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }
}
#endregion

#region Save
public abstract class SaveSingleCommand<TEntity, TVM> : SaveSingleCommand<TEntity, Guid, TVM>
    where TEntity : BaseEntity
    where TVM : BaseEntityVM, IMapFrom<TEntity>
{
}
public abstract class SaveSingleCommandHandler<TEntity, TVM, TDbContext, TCommand> : SaveSingleCommandHandler<TEntity, Guid, TVM, TDbContext, TCommand>
    where TCommand : SaveSingleCommand<TEntity, TVM>
    where TEntity : BaseEntity
    where TVM : BaseEntityVM, IMapFrom<TEntity>
    where TDbContext : ICoreDbContext
{
    public SaveSingleCommandHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }
}
#endregion

#region Delete
public abstract class DeleteSingleCommand<TEntity> : DeleteSingleCommand<TEntity, Guid>
    where TEntity : BaseEntity<Guid>
{

}
public abstract class DeleteSingleCommandHandler<TEntity, TDbContext, TCommand> : DeleteSingleCommandHandler<TEntity, Guid, TDbContext, TCommand>, IRequestHandler<TCommand, BaseVM>
    where TCommand : DeleteSingleCommand<TEntity>
    where TEntity : BaseEntity<Guid>
    where TDbContext : ICoreDbContext
{
    protected DeleteSingleCommandHandler(TDbContext appDbContext): base(appDbContext)
    {
    }
}
#endregion