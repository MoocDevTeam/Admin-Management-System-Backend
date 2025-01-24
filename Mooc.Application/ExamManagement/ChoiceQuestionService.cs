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
        var entity = await _dbContext.ChoiceQuestion
            .Include(q => q.Options)
            .Include(q => q.QuestionType)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (entity == null)
        {
            throw new UserFriendlyException($"Choice question with ID {id} not found");
        }

        return _mapper.Map<ChoiceQuestionDto>(entity);
    }

    public async Task<PagedResultDto<ChoiceQuestionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        var query = CreateFilteredQuery(input);
        
        var totalCount = await query.CountAsync();
        
        var entities = new List<ChoiceQuestion>();
        var entityDtos = new List<ChoiceQuestionDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            entities = await query.ToListAsync();
            entityDtos = _mapper.Map<List<ChoiceQuestionDto>>(entities);
        }

        return new PagedResultDto<ChoiceQuestionDto>(
            totalCount,
            entityDtos
        );
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

            // Create options
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
                    
                    // Generate ID using Snowflake algorithm
                    SetIdForLong(option);
                    
                    await _dbContext.Option.AddAsync(option);
                }
                
                await _dbContext.SaveChangesAsync();
            }

            // Reload complete data
            var result = await _dbContext.ChoiceQuestion
                .Include(q => q.Options)
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
            .Include(x => x.Options)
            .Include(x => x.QuestionType);
    }

    public override async Task<ChoiceQuestionDto> UpdateAsync(long id, UpdateChoiceQuestionDto input)
    {
        var existingQuestion = await _dbContext.ChoiceQuestion
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (existingQuestion == null)
        {
            throw new UserFriendlyException($"Choice question with ID {id} not found");
        }

        // Keep original CourseId and QuestionTypeId
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
        return _dbContext.Set<ChoiceQuestion>();  // Use _dbContext instead of DbContext
    }

    // Helper method
    private void SetIdForLong(Option option)
    {
        if (option.Id == 0)
        {
            option.Id = SnowflakeIdGeneratorUtil.NextId();
        }
    }
}