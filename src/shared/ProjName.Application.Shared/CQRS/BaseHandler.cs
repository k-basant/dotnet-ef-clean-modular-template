using ProjName.Application.Shared.Interfaces;
using AutoMapper;

namespace ProjName.Application.Shared.CQRS;

public abstract class BaseHandler<TDbContext> where TDbContext : ICoreDbContext
{
    protected TDbContext _dbContext;
    protected IMapper _mapper;
    public BaseHandler(TDbContext appDbContext, IMapper mapper)
    {
        _dbContext = appDbContext;
        _mapper = mapper;
    }
}

public abstract class BaseHandler<TDbContext, TCQ, TVM> : BaseHandler<TDbContext>, IRequestHandler<TCQ, TVM>
    where TDbContext : ICoreDbContext
    where TCQ : IRequest<TVM>
{
    protected BaseHandler(TDbContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
    {
    }
    public abstract Task<TVM> Handle(TCQ request, CancellationToken cancellationToken);
}