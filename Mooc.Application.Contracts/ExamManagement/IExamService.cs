namespace Mooc.Application.Contracts.ExamManagement;

public interface IExamService : ICrudService<ExamDto, ExamDto, long, FilterPagedResultRequestDto, CreateExamDto, UpdateExamDto>
{

}