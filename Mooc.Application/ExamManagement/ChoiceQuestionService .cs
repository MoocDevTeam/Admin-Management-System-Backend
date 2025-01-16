namespace Mooc.Application.ExamManagement;

public class ChoiceQuestionService : CrudService<ChoiceQuestion, ChoiceQuestionDto, ChoiceQuestionDto, long, FilterPagedResultRequestDto, CreateChoiceQuestionDto, UpdateChoiceQuestionDto>, IChoiceQuestionService, ITransientDependency
{
    public ChoiceQuestionService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<ChoiceQuestionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ChoiceQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<ChoiceQuestionDto> CreateAsync(CreateChoiceQuestionDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<ChoiceQuestionDto> UpdateAsync(long id, UpdateChoiceQuestionDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}