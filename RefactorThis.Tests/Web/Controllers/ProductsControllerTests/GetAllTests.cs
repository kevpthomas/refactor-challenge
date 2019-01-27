using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using NUnit.Framework;
using RefactorThis.ApiModels;
using RefactorThis.Controllers;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using Shouldly;

namespace RefactorThis.Tests.Web.Controllers.ProductsControllerTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProductsController"/> GetAll method.
    /// </summary>
    /// <remarks>
    /// Out of scope to test all controller methods. This demonstrates unit testing is possible
    /// given the current architecture.
    /// </remarks>
    public class GetAllTests : ControllersUnitTestBase<ProductsController>
    {
        [Test]
        public void GivenAtLeastOneProductInDatabase()
        {
            var product = new Product
            {
                Id = Faker.Random.Guid(),
                Name = Faker.Random.String(),
                Description = Faker.Random.String(),
                DeliveryPrice = Faker.Random.Decimal()
            };

            GetDependency<IProductRepository>().Setup(x => x.List()).Returns(new List<Product> { product });

            var dto = ((OkNegotiatedContentResult<ProductsDto>)TestInstance.GetAll()).Content.Items.First();
            dto.ShouldSatisfyAllConditions(
                () => dto.Id.ShouldBe(product.Id),
                () => dto.Name.ShouldBe(product.Name),
                () => dto.Description.ShouldBe(product.Description),
                () => dto.DeliveryPrice.ShouldBe(product.DeliveryPrice)
                );
        }

        [Test]
        public void GivenDatabaseError()
        {
            GetDependency<IProductRepository>()
                .Setup(x => x.List())
                .Throws(new DataException(Faker.Random.String()));

            TestInstance.GetAll().ShouldBeOfType<BadRequestResult>();
        }

        [Test]
        public void GivenServerError()
        {
            GetDependency<IProductRepository>()
                .Setup(x => x.List())
                .Throws(new Exception(Faker.Random.String()));

            TestInstance.GetAll().ShouldBeOfType<InternalServerErrorResult>();
        }
    }
}
