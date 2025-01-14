namespace Mooc.Application.ExamManagement;

public class ExamQuestionService : CrudService<ExamQuestion, ExamQuestionDto, ExamQuestionDto, long, FilterPagedResultRequestDto, CreateExamQuestionDto, UpdateExamQuestionDto>, IExamQuestionService, ITransientDependency
{
    public ExamQuestionService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<ExamQuestionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ExamQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<ExamQuestionDto> CreateAsync(CreateExamQuestionDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<ExamQuestionDto> UpdateAsync(long id, UpdateExamQuestionDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}