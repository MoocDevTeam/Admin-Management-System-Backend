namespace Mooc.Application.ExamManagement;

public class ExamService : CrudService<Exam, ExamDto, ExamDto, long, FilterPagedResultRequestDto, CreateExamDto, UpdateExamDto>, IExamService, ITransientDependency
{
    public ExamService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<ExamDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ExamDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<ExamDto> CreateAsync(CreateExamDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<ExamDto> UpdateAsync(long id, UpdateExamDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}