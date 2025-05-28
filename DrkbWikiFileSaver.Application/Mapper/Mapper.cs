using AutoMapper;
using DrkbWikiFileSaver.Application.UseCases.File.GetFile;

namespace DrkbWikiFileSaver.Application.Mapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Domain.Entities.File, GetFileResponse>()
            .MaxDepth(3)
            .ReverseMap();
    }
}