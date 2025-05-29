using AutoMapper;
using DrkbWikiFileSaver.Application.UseCases.File.Commands.UpdateFile;
using DrkbWikiFileSaver.Application.UseCases.File.GetFile;

namespace DrkbWikiFileSaver.Application.Mapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Domain.Entities.File, GetFileResponse>()
            .MaxDepth(3)
            .ReverseMap();
        CreateMap<Domain.Entities.File, UpdateFileResponse>()
            .MaxDepth(3)
            .ReverseMap();
    }
}