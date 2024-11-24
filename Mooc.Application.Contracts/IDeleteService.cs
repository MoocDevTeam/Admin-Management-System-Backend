namespace Mooc.Application.Contracts;

public interface IDeleteService<in TKey>
{
    Task DeleteAsync(TKey id);
}
