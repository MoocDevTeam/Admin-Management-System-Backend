using Microsoft.Extensions.Logging;
using Mooc.Core.Utils;
using Mooc.Shared.Constants.ExamManagement;

namespace Mooc.Application.ExamManagement;

public class MultipleChoiceQuestionService : CrudService<MultipleChoiceQuestion, MultipleChoiceQuestionDto, MultipleChoiceQuestionDto, long, FilterPagedResultRequestDto, CreateMultipleChoiceQuestionDto, UpdateMultipleChoiceQuestionDto>, IMultipleChoiceQuestionService, ITransientDependency
{
    private readonly MoocDBContext _dbContext;
    private readonly ILogger<MultipleChoiceQuestionService> _logger;
    private readonly IMapper _mapper;

    public MultipleChoiceQuestionService(MoocDBContext dbContext, IMapper mapper, ILogger<MultipleChoiceQuestionService> logger) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<MultipleChoiceQuestionDto> GetAsync(long id)
    {
        var entity = await _dbContext.MultipleChoiceQuestion
            .Include(q => q.Options)
            .Include(q => q.QuestionType)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (entity == null)
        {
            throw new UserFriendlyException($"Multiple choice question with ID {id} not found");
        }

        return _mapper.Map<MultipleChoiceQuestionDto>(entity);
    }

    public override async Task<MultipleChoiceQuestionDto> CreateAsync(CreateMultipleChoiceQuestionDto input)
    {
        if (input.Options?.Count < MultipleChoiceQuestionConsts.MINIMUM_OPTIONS_COUNT)
        {
            throw new UserFriendlyException($"Multiple choice question must have at least {MultipleChoiceQuestionConsts.MINIMUM_OPTIONS_COUNT} options");
        }

        // validate the correct answer format
        var correctAnswers = input.CorrectAnswers?.Split(',').Select(x => x.Trim()).ToList();
        if (correctAnswers == null || !correctAnswers.Any())
        {
            throw new UserFriendlyException("Multiple choice question must have at least one correct answer");
        }

        try 
        {
            _logger.LogInformation("Creating multiple choice question: {@Input}", input);
            
            var question = new MultipleChoiceQuestion
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
                CorrectAnswers = input.CorrectAnswers
            };
            
            await _dbContext.MultipleChoiceQuestion.AddAsync(question);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Multiple choice question created with ID: {Id}", question.Id);

            // Create options
            if (input.Options != null)
            {
                foreach (var optionDto in input.Options)
                {
                    var option = new Option
                    {
                        MultipleChoiceQuestionId = question.Id,
                        OptionOrder = optionDto.OptionOrder,
                        OptionValue = optionDto.OptionValue,
                        ErrorExplanation = correctAnswers.Contains(optionDto.OptionValue) ? "" : optionDto.ErrorExplanation,
                        CreatedByUserId = input.CreatedByUserId ?? 0,
                        UpdatedByUserId = input.CreatedByUserId ?? 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    SetIdForLong(option);
                    await _dbContext.Option.AddAsync(option);
                }
                
                await _dbContext.SaveChangesAsync();
            }

            var result = await _dbContext.MultipleChoiceQuestion
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == question.Id);

            return _mapper.Map<MultipleChoiceQuestionDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create multiple choice question. Input: {@Input}, Exception: {Message}", 
                input, 
                ex.InnerException?.Message ?? ex.Message);
            throw;
        }
    }

    protected override IQueryable<MultipleChoiceQuestion> CreateFilteredQuery(FilterPagedResultRequestDto input)
    {
        return base.CreateFilteredQuery(input)
            .Include(x => x.Options)
            .Include(x => x.QuestionType);
    }

    public override async Task<MultipleChoiceQuestionDto> UpdateAsync(long id, UpdateMultipleChoiceQuestionDto input)
    {
        var existingQuestion = await _dbContext.MultipleChoiceQuestion
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (existingQuestion == null)
        {
            throw new UserFriendlyException($"Multiple choice question with ID {id} not found");
        }

        input.CourseId = existingQuestion.CourseId;
        input.QuestionTypeId = existingQuestion.QuestionTypeId;
        
        try
        {
            var result = await base.UpdateAsync(id, input);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update multiple choice question. ID: {Id}, Input: {@Input}", id, input);
            throw;
        }
    }

    protected virtual DbSet<MultipleChoiceQuestion> GetDbSet()
    {
        return _dbContext.Set<MultipleChoiceQuestion>();
    }

    private void SetIdForLong(Option option)
    {
        if (option.Id == 0)
        {
            option.Id = SnowflakeIdGeneratorUtil.NextId();
        }
    }
}
