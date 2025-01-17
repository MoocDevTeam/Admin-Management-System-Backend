namespace Mooc.Application.ExamManagement;

public class ExamManagementProfile : Profile
{
    public ExamManagementProfile()
    {
        CreateMap<QuestionType, QuestionTypeDto>();
        CreateMap<CreateQuestionTypeDto, QuestionType>();
        CreateMap<UpdateQuestionTypeDto, QuestionType>();

        CreateMap<ChoiceQuestion, ChoiceQuestionDto>();
        CreateMap<CreateChoiceQuestionDto, ChoiceQuestion>();
        CreateMap<UpdateChoiceQuestionDto, ChoiceQuestion>();

        CreateMap<JudgementQuestion, JudgementQuestionDto>();
        CreateMap<CreateJudgementQuestionDto, JudgementQuestion>();
        CreateMap<UpdateJudgementQuestionDto, JudgementQuestion>();

        CreateMap<ShortAnsQuestion, ShortAnsQuestionDto>();
        CreateMap<CreateShortAnsQuestionDto, ShortAnsQuestion>();
        CreateMap<UpdateShortAnsQuestionDto, ShortAnsQuestion>();

        CreateMap<Option, OptionDto>();
        CreateMap<CreateOptionDto, Option>();
        CreateMap<UpdateOptionDto, Option>();

        CreateMap<ExamQuestion, ExamQuestionDto> ();
        CreateMap<CreateExamQuestionDto, ExamQuestion>();
        CreateMap<UpdateExamQuestionDto, ExamQuestion>();

        CreateMap<Exam, ExamDto>()
         .ForMember(dest => dest.ExamQuestion, opt => opt.MapFrom(src => src.ExamQuestion));
        CreateMap<CreateExamDto, Exam>();
        CreateMap<UpdateExamDto, Exam>();

        CreateMap<ExamPublish, ExamPublishDto>();
        CreateMap<CreateExamPublishDto, ExamPublish>();
        CreateMap<UpdateExamPublishDto, ExamPublish>();
    }
}
