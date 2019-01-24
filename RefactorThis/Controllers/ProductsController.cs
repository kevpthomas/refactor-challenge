using System;
using System.Web.Http;
using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Models;

namespace RefactorThis.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        /*
         * DONE Refactor to return my DTO objects wrapped in IHttpActionResult
         * DONE Then refactor to return generic error if anything goes wrong
         * Then refactor to use a stub logger
         * Then refactor to repository
         * Then refactor to correct HTTP codes, time permitting
         *     e.g., code 422 for bad ID submission (benandel.com)
         */

        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return ProcessRequestAndHandleException(() =>
            {
                var products = new Products();
                return Ok(Mapper.Map<ProductsDto>(products));
            });
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var products = new Products(name);
                return Ok(Mapper.Map<ProductsDto>(products));
            });
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var product = new Product(id);
                if (product.IsNew)
                    return NotFound();

                return Ok(Mapper.Map<ProductDto>(product));
            });
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            return ProcessRequestAndHandleException(() =>
            {
                product.Save();
            
                return Ok(Mapper.Map<ProductDto>(product));
            });
        }

        // TODO: remove Id from Product body
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            return ProcessRequestAndHandleException(() =>
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
            
                return Ok(Mapper.Map<ProductDto>(orig));
            });
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var product = new Product(id);
                product.Delete();
                return Ok();
            });
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOptions = new ProductOptions(productId);
                return Ok(Mapper.Map<ProductOptionsDto>(productOptions));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var option = new ProductOption(id);
                if (option.IsNew)
                    return NotFound();

                return Ok(Mapper.Map<ProductOptionDto>(option));
            });
        }

        // TODO: remove ProductId from ProductOption body
        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            return ProcessRequestAndHandleException(() =>
            {
                option.ProductId = productId;
                option.Save();

                return Ok(Mapper.Map<ProductOptionDto>(option));
            });
        }

        // TODO: remove Id and ProductId from ProductOption body
        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid id, ProductOption option)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var orig = new ProductOption(id)
                {
                    Name = option.Name,
                    Description = option.Description
                };

                if (!orig.IsNew)
                    orig.Save();

                return Ok(Mapper.Map<ProductOptionDto>(orig));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var opt = new ProductOption(id);
                opt.Delete();
                return Ok();
            });
        }

        /// <summary>
        /// Simple method to provide standardised exception handling for all requests.
        /// </summary>
        /// <param name="processRequest">
        /// Function to provide happy path request processing.
        /// </param>
        /// <returns>
        /// Output from happy path request processing, or an internal server error in the event
        /// of an unhandled exception.
        /// </returns>
        /// <remarks>
        /// This method ensures no leakage of sensitive framework details.
        /// </remarks>
        private IHttpActionResult ProcessRequestAndHandleException(Func<IHttpActionResult> processRequest)
        {
            try
            {
                return processRequest();
            }
            catch
            {
                //TODO: catch and log the exception
                return InternalServerError();
            }
        }
    }
}
