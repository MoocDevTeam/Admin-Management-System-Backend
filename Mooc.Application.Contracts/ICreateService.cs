namespace Mooc.Application.Contracts;

public interface ICreateService<TGetOutputDto, in TCreateInput>
{
    Task<TGetOutputDto> CreateAsync(TCreateInput input);
}
