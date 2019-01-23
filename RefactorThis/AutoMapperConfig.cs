using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Models;

namespace RefactorThis
{
    public static class AutoMapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Products, ProductsDto>();

                cfg.CreateMap<ProductOption, ProductOptionDto>();
                cfg.CreateMap<ProductOptions, ProductOptionsDto>();
            });
        }
    }
}