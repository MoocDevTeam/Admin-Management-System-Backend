namespace Mooc.Application.Contracts
{
    public interface IReadOnlyService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput>
    {
        Task<TGetOutputDto> GetAsync(TKey id);

        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
    }
}
