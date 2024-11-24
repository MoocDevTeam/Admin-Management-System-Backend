namespace Mooc.Application.Contracts;

public interface ICrudService<TEntityDto, in TKey>
{

}

public interface ICrudService<TEntityDto, in TKey, in TGetListInput>
    : ICrudService<TEntityDto, TKey, TGetListInput, TEntityDto>
{

}

public interface ICrudService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
    : ICrudService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
{

}
public interface ICrudService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
: ICrudService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
{

}

public interface ICrudService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
    : IReadOnlyService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
      ICreateService<TGetOutputDto, TCreateInput>,
      IUpdateService<TGetOutputDto, TKey, TUpdateInput>,
      IDeleteService<TKey>
{

}