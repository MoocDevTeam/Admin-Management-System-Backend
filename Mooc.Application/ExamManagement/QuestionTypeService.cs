using Mooc.Application.Contracts.ExamManagement.Dto.QuestionType;
using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Application.ExamManagement;

public class QuestionTypeService : CrudService<QuestionType, QuestionTypeDto, QuestionTypeDto, long, FilterPagedResultRequestDto, CreateQuestionTypeDto, UpdateQuestionTypeDto>, IQuestionTypeService, ITransientDependency
{

    public QuestionTypeService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {

    }

    public override async Task<QuestionTypeDto> CreateAsync(CreateQuestionTypeDto input)
    {
        //Check if the QuestionType Name existing
        var existingType = await GetDbSet().FirstOrDefaultAsync(qt => qt.QuestionTypeName == input.QuestionTypeName);
        if (existingType != null)
        {
            throw new EntityAlreadyExistsException($"QuestionType {input.QuestionTypeName} is exists.");
        }

        return await base.CreateAsync(input);
    }

    public override async Task<QuestionTypeDto> UpdateAsync(long id, UpdateQuestionTypeDto input)
    {
        //Check if there has a same QuestionType Name.
        var existingType = await GetDbSet().FirstOrDefaultAsync(qt => qt.QuestionTypeName == input.QuestionTypeName && qt.Id != id);
        if (existingType != null) {
            throw new EntityAlreadyExistsException($"There is another QuestionTypeName same as {input.QuestionTypeName}.");
        }

        return await base.UpdateAsync(id, input);
    }

    public override async Task DeleteAsync(long id)
    {
        await base.DeleteAsync(id);
    }

    public async Task<QuestionTypeDto> GetQuestionTypeByNameAsync(string questionTypeName)
    {
        //Check if the QuestionType Name not exists.
        var entity = await GetDbSet().FirstOrDefaultAsync(qt => qt.QuestionTypeName == questionTypeName);
        if (entity == null) 
        {
            throw new EntityNotFoundException($"This QuestionType Name of {questionTypeName} not found.");
        }
        
        return MapToGetOutputDto(entity);
    }

}


