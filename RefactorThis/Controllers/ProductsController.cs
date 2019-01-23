using System;
using System.Net;
using System.Web.Http;
using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Models;

namespace RefactorThis.Controllers
{
    // TODO: for all endpoints, return IHttpActionResult (e.g., base.Ok) with relevant model
    // TODO: return code 422 for bad ID submission (benandel.com)
    // TODO: refactor to use AutoMapper and Repository; also need IoC
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        /*
         * Refactor to return my DTO objects wrapped in IHttpActionResult
         *   needs AutoMapper
         * Then refactor to repository
         * Then refactor to correct HTTP codes, time permitting
         */

        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var products = new Products();
            return Ok(Mapper.Map<ProductsDto>(products));
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            var products = new Products(name);
            return Ok(Mapper.Map<ProductsDto>(products));
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            var product = new Product(id);
            if (product.IsNew)
                return NotFound();

            return Ok(Mapper.Map<ProductDto>(product));
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            product.Save();
        }

        // TODO: remove Id from Product body
        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
            return Ok();
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var productOptions = new ProductOptions(productId);
            return Ok(Mapper.Map<ProductOptionsDto>(productOptions));
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                return NotFound();

            return Ok(Mapper.Map<ProductOptionDto>(option));
        }

        // TODO: remove ProductId from ProductOption body
        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        // TODO: remove Id and ProductId from ProductOption body
        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
            return Ok();
        }
    }
}
