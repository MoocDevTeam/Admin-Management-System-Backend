namespace Mooc.Application.ExamManagement;

public class JudgementQuestionService : CrudService<JudgementQuestion, JudgementQuestionDto, JudgementQuestionDto, long, FilterPagedResultRequestDto, CreateJudgementQuestionDto, UpdateJudgementQuestionDto>, IJudgementQuestionService, ITransientDependency
{
    public JudgementQuestionService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<JudgementQuestionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<JudgementQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<JudgementQuestionDto> CreateAsync(CreateJudgementQuestionDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<JudgementQuestionDto> UpdateAsync(long id, UpdateJudgementQuestionDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}