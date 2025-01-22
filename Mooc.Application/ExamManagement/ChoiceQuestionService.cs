using Microsoft.Extensions.Logging;
using Mooc.Core.Utils;
namespace Mooc.Application.ExamManagement;

public class ChoiceQuestionService : CrudService<ChoiceQuestion, ChoiceQuestionDto, ChoiceQuestionDto, long, FilterPagedResultRequestDto, CreateChoiceQuestionDto, UpdateChoiceQuestionDto>, IChoiceQuestionService, ITransientDependency
{
    private const int REQUIRED_OPTIONS_COUNT = 4;
    private readonly MoocDBContext _dbContext;
    private readonly ILogger<ChoiceQuestionService> _logger;
    private readonly IMapper _mapper;

    public ChoiceQuestionService(MoocDBContext dbContext, IMapper mapper, ILogger<ChoiceQuestionService> logger) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ChoiceQuestionDto> GetAsync(long id)
    {
        return await base.GetAsync(id);
    }

    public async Task<PagedResultDto<ChoiceQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public override async Task<ChoiceQuestionDto> CreateAsync(CreateChoiceQuestionDto input)
    {
        if (input.Options?.Count != REQUIRED_OPTIONS_COUNT)
        {
            throw new UserFriendlyException($"Choice question must have exactly {REQUIRED_OPTIONS_COUNT} options");
        }

        try 
        {
            _logger.LogInformation("Creating choice question: {@Input}", input);
            
            var question = new ChoiceQuestion
            {
                Id = SnowflakeIdGeneratorUtil.NextId(),
                CourseId = input.CourseId,
                CreatedByUserId = input.CreatedByUserId ?? 0,
                UpdatedByUserId = input.CreatedByUserId ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                QuestionBody = input.QuestionBody,
                QuestionTitle = input.QuestionTitle,
                Marks = input.Marks,
                QuestionTypeId = input.QuestionTypeId,
                CorrectAnswer = input.CorrectAnswer
            };
            
            await _dbContext.ChoiceQuestion.AddAsync(question);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Choice question created with ID: {Id}", question.Id);

            // 创建选项
            if (input.Options != null)
            {
                foreach (var optionDto in input.Options)
                {
                    var option = new Option
                    {
                        ChoiceQuestionId = question.Id,
                        OptionOrder = optionDto.OptionOrder,
                        OptionValue = optionDto.OptionValue,
                        ErrorExplanation = optionDto.OptionValue == input.CorrectAnswer ? "" : optionDto.ErrorExplanation,
                        CreatedByUserId = input.CreatedByUserId ?? 0,
                        UpdatedByUserId = input.CreatedByUserId ?? 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    // 使用雪花算法生成ID
                    SetIdForLong(option);
                    
                    await _dbContext.Option.AddAsync(option);
                }
                
                await _dbContext.SaveChangesAsync();
            }

            // 重新加载完整数据
            var result = await _dbContext.ChoiceQuestion
                .Include(q => q.Option)
                .FirstOrDefaultAsync(q => q.Id == question.Id);

            return _mapper.Map<ChoiceQuestionDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create choice question. Input: {@Input}, Exception: {Message}", 
                input, 
                ex.InnerException?.Message ?? ex.Message);
            throw;
        }
    }

    protected override IQueryable<ChoiceQuestion> CreateFilteredQuery(FilterPagedResultRequestDto input)
    {
        return base.CreateFilteredQuery(input)
            .Include(x => x.Option)
            .Include(x => x.QuestionType);
    }

    public override async Task<ChoiceQuestionDto> UpdateAsync(long id, UpdateChoiceQuestionDto input)
    {
        var existingQuestion = await _dbContext.ChoiceQuestion
            .Include(q => q.Option)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (existingQuestion == null)
        {
            throw new UserFriendlyException($"Choice question with ID {id} not found");
        }

        // 保留原有的 CourseId 和 QuestionTypeId
        input.CourseId = existingQuestion.CourseId;
        input.QuestionTypeId = existingQuestion.QuestionTypeId;
        
        try
        {
            var result = await base.UpdateAsync(id, input);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update choice question. ID: {Id}, Input: {@Input}", id, input);
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }

    protected virtual DbSet<ChoiceQuestion> GetDbSet()
    {
        return _dbContext.Set<ChoiceQuestion>();  // 使用 _dbContext 而不是 DbContext
    }

    // 添加这个辅助方法
    private void SetIdForLong(Option option)
    {
        if (option.Id == 0)
        {
            option.Id = SnowflakeIdGeneratorUtil.NextId();
        }
    }

}