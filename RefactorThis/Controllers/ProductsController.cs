using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        /*
         * DONE Refactor to return my DTO objects wrapped in IHttpActionResult
         * DONE Then refactor to return generic error if anything goes wrong
         * DONE Then refactor to use a stub logger
         * DONE Then refactor so that all endpoints request only the necessary details in the DTO model
         * DONE Then refactor to repository
         */

        private readonly ILogger _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductsController(ILogger logger,
            IProductRepository productRepository,
            IProductOptionRepository productOptionRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return ProcessRequestAndHandleException(() =>
            {
                var products = _productRepository.List();
                var productsDto = new ProductsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductDto>>(products)
                };

                return Ok(productsDto);
            });
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var products = _productRepository.GetByName(name);

                var productsDto = new ProductsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductDto>>(products)
                };

                return Ok(productsDto);
            });
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var product = _productRepository.GetById(id);

                if (product == null)
                    return NotFound();

                return Ok(Mapper.Map<ProductDto>(product));
            });
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(ProductDto product)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var insertedProduct = _productRepository.Add(Mapper.Map<Product>(product));
            
                return Ok(Mapper.Map<ProductDto>(insertedProduct));
            });
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, ProductUpdateDto productUpdate)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var product = Mapper.Map<Product>(productUpdate);
                product.Id = id;

                var updatedProduct = _productRepository.Update(product);
            
                return Ok(Mapper.Map<ProductDto>(updatedProduct));
            });
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                _productRepository.Delete(Product.FromId(id));

                return Ok();
            });
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOptions = _productOptionRepository.List(productId);

                var productOptionsDto = new ProductOptionsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductOptionDto>>(productOptions)
                };

                return Ok(productOptionsDto);
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOption = _productOptionRepository.GetById(productId, id);

                if (productOption == null)
                    return NotFound();

                return Ok(Mapper.Map<ProductOptionDto>(productOption));
            });
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOptionInsertDto option)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOption = Mapper.Map<ProductOption>(option);
                productOption.ProductId = productId;

                var insertedProductOption = _productOptionRepository.Add(productOption);
            
                return Ok(Mapper.Map<ProductOptionDto>(insertedProductOption));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOptionUpdateDto option)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOption = Mapper.Map<ProductOption>(option);
                productOption.Id = id;
                productOption.ProductId = productId;

                var updatedProductOption = _productOptionRepository.Update(productOption);
            
                return Ok(Mapper.Map<ProductOptionDto>(updatedProductOption));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            return ProcessRequestAndHandleException(() =>
            {
                _productOptionRepository.Delete(ProductOption.FromId(productId, id));

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
            catch (DataException ex)
            {
                // making an assumption that any SQL exception is the result of invalid parameters in the request
                _logger.Log(ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return InternalServerError();
            }
        }
    }
}
