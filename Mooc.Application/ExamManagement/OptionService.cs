namespace Mooc.Application.ExamManagement;

public class OptionService : CrudService<Option, OptionDto, OptionDto, long, FilterPagedResultRequestDto, CreateOptionDto, UpdateOptionDto>, IOptionService, ITransientDependency
{
    public OptionService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<OptionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<OptionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<OptionDto> CreateAsync(CreateOptionDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<OptionDto> UpdateAsync(long id, UpdateOptionDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}