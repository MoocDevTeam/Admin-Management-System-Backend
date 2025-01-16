using Microsoft.EntityFrameworkCore;

namespace Mooc.Application.ExamManagement;

public class ExamService : CrudService<Exam, ExamDto, ExamDto, long, FilterPagedResultRequestDto, CreateExamDto, UpdateExamDto>, IExamService, ITransientDependency
{
    private readonly MoocDBContext _moocDBContext;
    private readonly IMapper _mapper;

    public ExamService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _moocDBContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ExamDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<List<ExamDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        var exams = await this.McDBContext.Exam
         .Include(c => c.ExamQuestion) // Include exam question to load Exam
         .ToListAsync();

        if (exams.Count == 0)
            return new List<ExamDto>();

        var examOutput = this._mapper.Map<List<ExamDto>>(exams);


        return examOutput;

        /* return await base.GetListAsync(input);*/
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