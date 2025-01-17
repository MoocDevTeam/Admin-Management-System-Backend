namespace Mooc.Application.Contracts.ExamManagement;

public interface IExamQuestionService : ICrudService<ExamQuestionDto, ExamQuestionDto, long, FilterPagedResultRequestDto, CreateExamQuestionDto, UpdateExamQuestionDto>
{

}