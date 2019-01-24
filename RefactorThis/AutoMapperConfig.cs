using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Core.Entities;
using RefactorThis.Models;

namespace RefactorThis
{
    public static class AutoMapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductEntity, ProductDto>();


                //TODO: remove this Product/ProductDto mapping
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<Products, ProductsDto>();

                cfg.CreateMap<ProductOption, ProductOptionDto>();
                cfg.CreateMap<ProductOptions, ProductOptionsDto>();
            });
        }
    }
}