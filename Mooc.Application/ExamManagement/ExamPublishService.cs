namespace Mooc.Application.ExamManagement;

public class ExamPublishService : CrudService<ExamPublish, ExamPublishDto, ExamPublishDto, long, FilterPagedResultRequestDto, CreateExamPublishDto, UpdateExamPublishDto>, IExamPublishService, ITransientDependency
{
    public ExamPublishService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public async Task<ExamPublishDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ExamPublishDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<ExamPublishDto> CreateAsync(CreateExamPublishDto input)
    {
        return await base.CreateAsync(input);
    }

    public async Task<ExamPublishDto> UpdateAsync(long id, UpdateExamPublishDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}