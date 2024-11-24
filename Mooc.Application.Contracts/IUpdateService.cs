namespace Mooc.Application.Contracts;

public interface IUpdateService<TGetOutputDto, in TKey, in TUpdateInput>
{
    Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);
}
