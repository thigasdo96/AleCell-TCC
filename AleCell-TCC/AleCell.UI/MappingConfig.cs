using AutoMapper;
using AleCell.UI.DTOs;
using AleCell.UI.ViewModels;

namespace AleCell.UI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CategoriaDto, CategoriaVM>()
            .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Foto))
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        CreateMap<CategoriaVM, CategoriaDto>()
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        CreateMap<ProdutoDto, ProdutoVM>()
            .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Foto))
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        CreateMap<ProdutoVM, ProdutoDto>()
            .ForMember(dest => dest.Foto, opt => opt.Ignore());
    }
}
