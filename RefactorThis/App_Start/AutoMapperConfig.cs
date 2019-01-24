using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Core.Entities;

namespace RefactorThis
{
    /// <summary>
    /// Provides mechanism for AutoMapper model mappings.
    /// </summary>
    public static class AutoMapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>().ReverseMap();

                cfg.CreateMap<ProductUpdateDto, Product>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());


                cfg.CreateMap<ProductOption, ProductOptionDto>().ReverseMap();

                cfg.CreateMap<ProductOptionInsertDto, ProductOption>()
                    .ForMember(dest => dest.ProductId, opt => opt.Ignore());

                cfg.CreateMap<ProductOptionUpdateDto, ProductOption>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductId, opt => opt.Ignore());
            });
        }
    }
}