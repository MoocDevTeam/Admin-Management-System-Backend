using Microsoft.Extensions.Logging;
using Mooc.Core.Utils;
using Mooc.Shared.Constants.ExamManagement;
using Microsoft.AspNetCore.Http;

namespace Mooc.Application.ExamManagement;

public class MultipleChoiceQuestionService : CrudService<MultipleChoiceQuestion, MultipleChoiceQuestionDto, MultipleChoiceQuestionDto, long, FilterPagedResultRequestDto, CreateMultipleChoiceQuestionDto, UpdateMultipleChoiceQuestionDto>, IMultipleChoiceQuestionService, ITransientDependency
{
    private readonly MoocDBContext _dbContext;
    private readonly ILogger<MultipleChoiceQuestionService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MultipleChoiceQuestionService(
        MoocDBContext dbContext, 
        IMapper mapper, 
        ILogger<MultipleChoiceQuestionService> logger,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
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
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UserFriendlyException("User is not authenticated");
        }

        if (input.Options?.Count < MultipleChoiceQuestionConsts.MINIMUM_OPTIONS_COUNT)
        {
            throw new UserFriendlyException($"Multiple choice question must have at least {MultipleChoiceQuestionConsts.MINIMUM_OPTIONS_COUNT} options");
        }

        var correctAnswers = input.CorrectAnswers?.Split(',').Select(x => x.Trim()).ToList();
        if (correctAnswers == null || !correctAnswers.Any())
        {
            throw new UserFriendlyException("Multiple choice question must have at least one correct answer");
        }

        try 
        {
            _logger.LogInformation("Creating multiple choice question: {@Input}", input);
            
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                throw new UserFriendlyException("User is not authenticated");
            }
            var currentUser = await _userService.GetByUserNameAsync(userName);
            
            var question = new MultipleChoiceQuestion
            {
                CourseId = input.CourseId,
                CreatedByUserId = currentUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                QuestionBody = input.QuestionBody,
                QuestionTitle = input.QuestionTitle,
                Marks = input.Marks,
                QuestionTypeId = input.QuestionTypeId,
                CorrectAnswers = input.CorrectAnswers
            };
            
            base.SetIdForLong(question);
            await _dbContext.MultipleChoiceQuestion.AddAsync(question);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Multiple choice question created with ID: {Id}", question.Id);

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
                        CreatedByUserId = currentUser.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    base.SetIdForLong(option);
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
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var existingQuestion = await _dbContext.MultipleChoiceQuestion
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuestion == null)
            {
                throw new UserFriendlyException($"Multiple choice question with ID {id} not found");
            }

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                throw new UserFriendlyException("User is not authenticated");
            }
            var currentUser = await _userService.GetByUserNameAsync(userName);

            existingQuestion.QuestionBody = input.QuestionBody;
            existingQuestion.QuestionTitle = input.QuestionTitle;
            existingQuestion.Marks = input.Marks;
            existingQuestion.CorrectAnswers = input.CorrectAnswers;
            existingQuestion.UpdatedByUserId = currentUser.Id;
            existingQuestion.UpdatedAt = DateTime.UtcNow;

            _dbContext.MultipleChoiceQuestion.Update(existingQuestion);

            if (input.Options != null && input.Options.Any())
            {
                var correctAnswers = input.CorrectAnswers?.Split(',').Select(x => x.Trim()).ToList();
                
                foreach (var optionDto in input.Options)
                {
                    var existingOption = existingQuestion.Options.FirstOrDefault(o => o.Id == optionDto.Id);
                    if (existingOption != null)
                    {
                        // update existing option
                        existingOption.OptionOrder = optionDto.OptionOrder;
                        existingOption.OptionValue = optionDto.OptionValue;
                        existingOption.ErrorExplanation = correctAnswers.Contains(optionDto.OptionValue) ? "" : optionDto.ErrorExplanation;
                        existingOption.UpdatedByUserId = currentUser.Id;
                        existingOption.UpdatedAt = DateTime.UtcNow;

                        _dbContext.Option.Update(existingOption);
                    }
                    else
                    {
                        // create new option
                        var newOption = new Option
                        {
                            MultipleChoiceQuestionId = id,
                            OptionOrder = optionDto.OptionOrder,
                            OptionValue = optionDto.OptionValue,
                            ErrorExplanation = correctAnswers.Contains(optionDto.OptionValue) ? "" : optionDto.ErrorExplanation,
                            CreatedByUserId = currentUser.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        base.SetIdForLong(newOption);
                        await _dbContext.Option.AddAsync(newOption);
                    }
                }

                // delete unnecessary options
                var optionIdsToKeep = input.Options.Select(o => o.Id).ToList();
                await _dbContext.Option
                    .Where(o => o.MultipleChoiceQuestionId == id && !optionIdsToKeep.Contains(o.Id))
                    .ExecuteDeleteAsync();
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<MultipleChoiceQuestionDto>(existingQuestion);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to update multiple choice question. ID: {Id}, Input: {@Input}", id, input);
            throw;
        }
    }

    protected virtual DbSet<MultipleChoiceQuestion> GetDbSet()
    {
        return _dbContext.Set<MultipleChoiceQuestion>();
    }
}
