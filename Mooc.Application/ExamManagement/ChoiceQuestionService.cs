using Microsoft.Extensions.Logging;
using Mooc.Core.Utils;
using Mooc.Shared.Constants.ExamManagement;
using Mooc.Application.Admin;
using Microsoft.AspNetCore.Http;
namespace Mooc.Application.ExamManagement;


public class ChoiceQuestionService : CrudService<ChoiceQuestion, ChoiceQuestionDto, ChoiceQuestionDto, long, FilterPagedResultRequestDto, CreateChoiceQuestionDto, UpdateChoiceQuestionDto>, IChoiceQuestionService, ITransientDependency
{
    private readonly MoocDBContext _dbContext;
    private readonly ILogger<ChoiceQuestionService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionService _optionService;

    public ChoiceQuestionService(MoocDBContext dbContext, IMapper mapper, ILogger<ChoiceQuestionService> logger, IUserService userService, IHttpContextAccessor httpContextAccessor, IOptionService optionService) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _optionService = optionService;
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
        try 
        {
            _logger.LogInformation("Creating choice question: {@Input}", input);
            
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var currentUser = await _userService.GetByUserNameAsync(userName);
            
            var question = new ChoiceQuestion
            {
                CourseId = input.CourseId,
                CreatedByUserId = currentUser.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                QuestionBody = input.QuestionBody,
                QuestionTitle = input.QuestionTitle,
                Marks = input.Marks,
                QuestionTypeId = input.QuestionTypeId,
                CorrectAnswer = input.CorrectAnswer
            };
            
            base.SetIdForLong(question);
            await _dbContext.ChoiceQuestion.AddAsync(question);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Choice question created with ID: {Id}", question.Id);

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
                        CreatedByUserId = currentUser.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    base.SetIdForLong(option);
                    await _dbContext.Option.AddAsync(option);
                }
                
                await _dbContext.SaveChangesAsync();
            }

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
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var existingQuestion = await _dbContext.ChoiceQuestion
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuestion == null)
            {
                throw new UserFriendlyException($"Choice question with ID {id} not found");
            }

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                throw new UserFriendlyException("User is not authenticated");
            }
            var currentUser = await _userService.GetByUserNameAsync(userName);

            // update question
            existingQuestion.CorrectAnswer = input.CorrectAnswer;
            existingQuestion.QuestionBody = input.QuestionBody;
            existingQuestion.QuestionTitle = input.QuestionTitle;
            existingQuestion.UpdatedByUserId = currentUser.Id;
            existingQuestion.UpdatedAt = DateTime.UtcNow;

            _dbContext.ChoiceQuestion.Update(existingQuestion);

            if (input.Options != null && input.Options.Any())
            {
                foreach (var optionDto in input.Options)
                {
                    var existingOption = existingQuestion.Options.FirstOrDefault(o => o.Id == optionDto.Id);
                    if (existingOption != null)
                    {
                        // update existing option
                        existingOption.OptionOrder = optionDto.OptionOrder;
                        existingOption.OptionValue = optionDto.OptionValue;
                        existingOption.ErrorExplanation = optionDto.OptionValue == input.CorrectAnswer ? "" : optionDto.ErrorExplanation;
                        existingOption.UpdatedByUserId = currentUser.Id;
                        existingOption.UpdatedAt = DateTime.UtcNow;

                        _dbContext.Option.Update(existingOption);
                    }
                    else
                    {
                        // create new option
                        var newOption = new Option
                        {
                            ChoiceQuestionId = id,
                            OptionOrder = optionDto.OptionOrder,
                            OptionValue = optionDto.OptionValue,
                            ErrorExplanation = optionDto.OptionValue == input.CorrectAnswer ? "" : optionDto.ErrorExplanation,
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
                    .Where(o => o.ChoiceQuestionId == id && !optionIdsToKeep.Contains(o.Id))
                    .ExecuteDeleteAsync();
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<ChoiceQuestionDto>(existingQuestion);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
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
        return _dbContext.Set<ChoiceQuestion>();
    }
}