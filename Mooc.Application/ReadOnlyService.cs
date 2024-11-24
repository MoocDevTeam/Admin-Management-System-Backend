using AutoMapper.Internal.Mappers;
using Mooc.Application.Contracts;
using Mooc.Application.Contracts.Dto;
using Mooc.Core.ExceptionHandling;

namespace Mooc.Application;

public abstract class ReadOnlyService<TEntity, TEntityDto, TKey>
    : ReadOnlyService<TEntity, TEntityDto, TEntityDto, TKey, FilterPagedResultRequestDto>
    where TEntity : BaseEntity
{
    protected ReadOnlyService(MoocDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {

    }
}

public abstract class ReadOnlyService<TEntity, TEntityDto, TKey, TGetListInput>
    : ReadOnlyService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>
    where TEntity : BaseEntity
{
    protected ReadOnlyService(MoocDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {

    }
}


public abstract class ReadOnlyService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput> :
   IReadOnlyService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    where TEntity : BaseEntity
{

    protected MoocDBContext McDBContext { get; }
    protected IMapper Mapper { get; }

    public ReadOnlyService(MoocDBContext dbContext, IMapper mapper)
    {
        this.McDBContext = dbContext;
        this.Mapper = mapper;
    }

    protected virtual async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await GetDbSet().FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
        return this.McDBContext.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet().AsQueryable().AsNoTracking();
    }

    /// <summary>
    /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
    /// It should filter query if needed, but should not do sorting or paging.
    /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
    /// methods.
    /// </summary>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetListInput input)
    {
        return GetQueryable();
    }

    public async Task<TGetOutputDto> GetAsync(TKey id)
    {
        var entity = await GetEntityByIdAsync(id);
        return MapToGetOutputDto(entity);
    }


    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetOutputDto"/>.
    /// It uses <see cref="Mapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetOutputDto MapToGetOutputDto(TEntity entity)
    {
        return this.Mapper.Map<TEntity, TGetOutputDto>(entity);
    }

    /// <summary>
    /// Applies sorting if no sorting specified but a limited result requested.
    /// </summary>
    /// <param name="query">The query.</param>
    protected virtual IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsSubclassOf(typeof(BaseEntity)))
        {
            return query.OrderBy(e => ((BaseEntity)e).Id);
        }
        throw new MoocException("No sorting specified but this query requires sorting. Override the ApplySorting or the ApplyDefaultSorting method for your application service derived from ReadOnlyService!");
    }

    /// <summary>
    /// Should apply sorting if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to sort query if available
        if (input is ISortedResultRequest sortInput)
        {
            if (!string.IsNullOrEmpty(sortInput.Sorting))
            {
                return query.OrderBy(sortInput.Sorting!);
            }
        }

        //IQueryable.Task requires sorting, so we should sort if Take will be used.
        if (input is IPagedResultRequest)
        {
            return ApplyDefaultSorting(query);
        }

        //No sorting
        return query;
    }

    /// <summary>
    /// Should apply paging if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to use paging if available
        if (input is IPagedResultRequest pagedInput)
        {
            return query.PageBy(pagedInput);
        }
        //No paging
        return query;
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/>.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetListOutputDto MapToGetListOutputDto(TEntity entity)
    {
        return this.Mapper.Map<TEntity, TGetListOutputDto>(entity);
    }

    /// <summary>
    /// Maps a list of <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/> objects.
    /// It uses <see cref="MapToGetListOutputDtoAsync"/> method for each item in the list.
    /// </summary>
    protected virtual List<TGetListOutputDto> MapToGetListOutputDtos(List<TEntity> entities)
    {
        var dtos = new List<TGetListOutputDto>();

        foreach (var entity in entities)
        {
            dtos.Add(MapToGetListOutputDto(entity));
        }
        return dtos;
    }

    public async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
    {
        var query = CreateFilteredQuery(input);
        var totalCount = await query.CountAsync();

        var entities = new List<TEntity>();
        var entityDtos = new List<TGetListOutputDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            entities = await query.ToListAsync();
            entityDtos = MapToGetListOutputDtos(entities);
        }

        return new PagedResultDto<TGetListOutputDto>(
            totalCount,
            entityDtos
        );
    }
}