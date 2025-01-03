using Mooc.Application.Contracts.ExamManagement.Dto.QuestionType;

namespace Mooc.Application.Contracts.ExamManagement
{
    public interface IQuestionTypeService: ICrudService<QuestionTypeDto, QuestionTypeDto, long, FilterPagedResultRequestDto, CreateQuestionTypeDto, UpdateQuestionTypeDto>
    {
        Task<QuestionTypeDto> GetQuestionTypeByNameAsync(string questionTypeName);
    }
}
