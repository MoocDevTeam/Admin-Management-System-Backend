using AutoMapper;
using System;

public class ChoiceQuestionProfile : Profile
{
    public ChoiceQuestionProfile()
    {
        // ChoiceQuestion mappings
        CreateMap<ChoiceQuestion, ChoiceQuestionDto>()
            .ForMember(dest => dest.Options, 
                opt => opt.MapFrom(src => src.Options));

        CreateMap<CreateChoiceQuestionDto, ChoiceQuestion>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateChoiceQuestionDto, ChoiceQuestion>();

        // Option mappings
        CreateMap<Option, OptionDto>();
        CreateMap<CreateOptionDto, Option>();
        CreateMap<UpdateOptionDto, Option>();
    }
}
