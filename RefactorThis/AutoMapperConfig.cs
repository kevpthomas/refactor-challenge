using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Core.Entities;
using RefactorThis.Models;
using ProductOption = RefactorThis.Models.ProductOption;

namespace RefactorThis
{
    public static class AutoMapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
                cfg.CreateMap<ProductUpdateDto, Product>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                //TODO: remove this ProductObsolete/ProductDto mapping
                cfg.CreateMap<ProductObsolete, ProductDto>();
                cfg.CreateMap<Products, ProductsDto>();

                cfg.CreateMap<ProductOption, ProductOptionDto>();
                cfg.CreateMap<ProductOptionsObsolete, ProductOptionsDto>();
            });
        }
    }
}