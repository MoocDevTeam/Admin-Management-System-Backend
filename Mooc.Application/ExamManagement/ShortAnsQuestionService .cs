namespace Mooc.Application.ExamManagement;

public class ShortAnsQuestionService : CrudService<ShortAnsQuestion, ShortAnsQuestionDto, ShortAnsQuestionDto, long, FilterPagedResultRequestDto, CreateShortAnsQuestionDto, UpdateShortAnsQuestionDto>, IShortAnsQuestionService, ITransientDependency
{
    public ShortAnsQuestionService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<ShortAnsQuestionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ShortAnsQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<ShortAnsQuestionDto> CreateAsync(CreateShortAnsQuestionDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<ShortAnsQuestionDto> UpdateAsync(long id, UpdateShortAnsQuestionDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}