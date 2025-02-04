using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mooc.Core.Utils;
using Mooc.Model.Entity;
using static Amazon.S3.Util.S3EventNotification;

namespace Mooc.Application.ExamManagement;

public class ExamService : CrudService<Exam, ExamDto, ExamDto, long, FilterPagedResultRequestDto, CreateExamDto, UpdateExamDto>, IExamService, ITransientDependency
{
    private readonly MoocDBContext _moocDBContext;
    private readonly ILogger<ExamService> _logger;
    private readonly IMapper _mapper;

    public ExamService(MoocDBContext dbContext, IMapper mapper, ILogger<ExamService> logger) : base(dbContext, mapper)
    {
        _moocDBContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Overrides the default CreateFilteredQuery method to include related entities while fetching exams
    /// </summary>
    /// <param name="input">Filter parameters for the query</param>
    /// <returns>IQueryable with related entities included</returns>
    protected override IQueryable<Exam> CreateFilteredQuery(FilterPagedResultRequestDto input)
    {
        return this.McDBContext.Exam
            .Include(x => x.ExamQuestions)
            .Include(x => x.ExamPublish);
    }

    public override async Task<ExamDto> GetAsync(long id)
    {

        var exam = await this.McDBContext.Exam
          .Include(x => x.ExamQuestions)
          .Include(c => c.ExamPublish)
          .FirstOrDefaultAsync(x => x.Id == id);

        if (exam == null)
            throw new EntityNotFoundException("CourseInstance not Found", $"CourseInstance with Id {id} is not found");
        return MapToGetOutputDto(exam);
    }

    public async Task<PagedResultDto<ExamDto>> GetListAsyncBeta(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public override async Task<ExamDto> CreateAsync(CreateExamDto input)
    {
        // You can create an exam without creating exam questions at the same time
        if (input.WithExamQuestion == false)
        {
            var newInput = new CreateExamDto
            {
                CourseId = input.CourseId,
                CreatedByUserId = input.CreatedByUserId,
                UpdatedByUserId = input.UpdatedByUserId,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt,
                ExamTitle = input.ExamTitle,
                Remark = input.Remark,
                ExaminationTime = input.ExaminationTime,
                AutoOrManual = input.AutoOrManual,
                TotalScore = input.TotalScore,
                TimePeriod = input.TimePeriod,
            };

            return await base.CreateAsync(newInput);
        }

        // You can create an exam and exam questions at the same time
        try
        {
            _logger.LogInformation("Creating exam: {@Input}", input);

            var exam = new Exam
            {
                Id = SnowflakeIdGeneratorUtil.NextId(),
                CourseId = input.CourseId,
                CreatedByUserId = input.CreatedByUserId | 0,
                UpdatedByUserId = input.CreatedByUserId | 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExamTitle = input.ExamTitle,
                Remark = input.Remark,
                ExaminationTime = input.ExaminationTime,
                AutoOrManual = input.AutoOrManual,
                TotalScore = input.TotalScore,
                TimePeriod = input.TimePeriod,
            };

            await _moocDBContext.Exam.AddAsync(exam);
            await _moocDBContext.SaveChangesAsync();

            _logger.LogInformation("Exam created with ID: {Id}", exam.Id);

            // Create options
            if (input.ExamQuestions != null)
            {
                foreach (var examQuestionDto in input.ExamQuestions)
                {
                    var examQuestion = new ExamQuestion
                    {
                        ExamId = exam.Id,
                        ChoiceQuestionId = examQuestionDto.ChoiceQuestionId,
                        JudgementQuestionId = examQuestionDto.JudgementQuestionId,
                        ShortAnsQuestionId = examQuestionDto.ShortAnsQuestionId,
                        Marks = examQuestionDto.Marks,
                        QuestionOrder = examQuestionDto.QuestionOrder,
                        CreatedByUserId = input.CreatedByUserId | 0,
                        UpdatedByUserId = input.CreatedByUserId | 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    // Generate ID using Snowflake algorithm
                    SetIdForLong(examQuestion);

                    await _moocDBContext.ExamQuestion.AddAsync(examQuestion);
                }

                await _moocDBContext.SaveChangesAsync();
            }

            // Reload complete data
            var result = await _moocDBContext.Exam
                .Include(e => e.ExamQuestions)
                .FirstOrDefaultAsync(e => e.Id == exam.Id);

            return _mapper.Map<ExamDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create choice question. Input: {@Input}, Exception: {Message}",
                input,
                ex.InnerException?.Message ?? ex.Message);
            throw;
        }
    }

    public override async Task<ExamDto> UpdateAsync(long id, UpdateExamDto input)
    {
        return await base.UpdateAsync(id, input);
    }

    public override async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }
}