using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> GetAll()
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var products = await _productRepository.ListAsync();
                var productsDto = new ProductsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductDto>>(products)
                };

                return Ok(productsDto);
            });
        }

        [Route("search")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var products = await _productRepository.GetByNameAsync(name);

                var productsDto = new ProductsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductDto>>(products)
                };

                return Ok(productsDto);
            });
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product == null)
                    return NotFound();

                return Ok(Mapper.Map<ProductDto>(product));
            });
        }

        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> Create(ProductDto product)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var insertedProduct = await _productRepository.AddAsync(Mapper.Map<Product>(product));
            
                return Ok(Mapper.Map<ProductDto>(insertedProduct));
            });
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update(Guid id, ProductUpdateDto productUpdate)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var product = Mapper.Map<Product>(productUpdate);
                product.Id = id;

                var updatedProduct = await _productRepository.UpdateAsync(product);
            
                return Ok(Mapper.Map<ProductDto>(updatedProduct));
            });
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                await _productRepository.DeleteAsync(Product.FromId(id));

                return Ok();
            });
        }

        [Route("{productId}/options")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOptions(Guid productId)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var productOptions = await _productOptionRepository.ListAsync(productId);

                var productOptionsDto = new ProductOptionsDto
                {
                    Items = Mapper.Map<IEnumerable<ProductOptionDto>>(productOptions)
                };

                return Ok(productOptionsDto);
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOption(Guid productId, Guid id)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var productOption = await _productOptionRepository.GetByIdAsync(productId, id);

                if (productOption == null)
                    return NotFound();

                return Ok(Mapper.Map<ProductOptionDto>(productOption));
            });
        }

        [Route("{productId}/options")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateOption(Guid productId, ProductOptionInsertDto option)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var productOption = Mapper.Map<ProductOption>(option);
                productOption.ProductId = productId;

                var insertedProductOption = await _productOptionRepository.AddAsync(productOption);
            
                return Ok(Mapper.Map<ProductOptionDto>(insertedProductOption));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateOption(Guid productId, Guid id, ProductOptionUpdateDto option)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                var productOption = Mapper.Map<ProductOption>(option);
                productOption.Id = id;
                productOption.ProductId = productId;

                var updatedProductOption = await _productOptionRepository.UpdateAsync(productOption);
            
                return Ok(Mapper.Map<ProductOptionDto>(updatedProductOption));
            });
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOption(Guid productId, Guid id)
        {
            return await ProcessRequestAndHandleExceptionAsync(async () =>
            {
                await _productOptionRepository.DeleteAsync(ProductOption.FromId(productId, id));

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
        /// Output from happy path request processing, or an alternate HTTP status code
        /// in the event of an exception.
        /// </returns>
        /// <remarks>
        /// This method ensures no leakage of sensitive framework details.
        /// </remarks>
        private async Task<IHttpActionResult> ProcessRequestAndHandleExceptionAsync(Func<Task<IHttpActionResult>> processRequest)
        {
            try
            {
                return await processRequest();
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
