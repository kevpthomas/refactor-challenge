using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using AutoMapper;
using RefactorThis.ApiModels;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Models;
using ProductOption = RefactorThis.Models.ProductOption;

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
         * Then refactor to repository
         * Then refactor to correct HTTP codes, time permitting
         *     e.g., code 422 for bad ID submission (benandel.com)
         */

        private readonly ILogger _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
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
                _productRepository.Delete(id);

                return Ok();
            });
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var productOptions = new ProductOptionsObsolete(productId);
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

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOptionInsertDto optionDto)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var orig = new ProductOption
                {
                    Id = optionDto.Id,
                    ProductId = productId,
                    Description = optionDto.Description,
                    Name = optionDto.Name
                };

                orig.Save();

                return Ok(Mapper.Map<ProductOptionDto>(orig));
            });
        }

        // TODO: should use supplied productId
        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid id, ProductOptionUpdateDto option)
        {
            return ProcessRequestAndHandleException(() =>
            {
                var orig = new ProductOption(id)
                {
                    Name = option.Name,
                    Description = option.Description
                };

                if (orig.IsNew)
                    return BadRequest();

                orig.Save();

                return Ok(Mapper.Map<ProductOptionDto>(orig));
            });
        }

        // TODO: should use supplied productId
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
            catch (DataException ex)
            {
                // making an assumption that any SQL exception is the result of invalid parameters in the request
                _logger.Log(ex);
                return BadRequest();
            }
            catch (SqlException ex)
            {
                //TODO: remove this catch statement after refactor to repository pattern

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
