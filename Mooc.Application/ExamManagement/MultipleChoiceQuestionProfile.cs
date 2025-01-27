using AutoMapper;
using System;

namespace Mooc.Application.ExamManagement;

public class MultipleChoiceQuestionProfile : Profile
{
    public MultipleChoiceQuestionProfile()
    {
        // MultipleChoiceQuestion mappings
        CreateMap<MultipleChoiceQuestion, MultipleChoiceQuestionDto>()
            .ForMember(dest => dest.Options, 
                opt => opt.MapFrom(src => src.Options));

        CreateMap<CreateMultipleChoiceQuestionDto, MultipleChoiceQuestion>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateMultipleChoiceQuestionDto, MultipleChoiceQuestion>();
    }
}
