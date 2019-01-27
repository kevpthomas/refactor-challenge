using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NPoco;
using NUnit.Framework;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Infrastructure.Data;
using RefactorThis.Infrastructure.Interfaces;
using Shouldly;

namespace RefactorThis.Tests.Infrastructure.Data.ProductRepositoryTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProductRepository"/> List method.
    /// </summary>
    /// <remarks>
    /// Out of scope to test all repository methods. This demonstrates unit testing is possible
    /// given the current architecture.
    /// </remarks>
    public class ListAsyncTests : UnitTestBase<ProductRepository>
    {
        [Test]
        public async Task GivenAtLeastOneProductInDatabase()
        {
            var product = new Product
            {
                Id = Faker.Random.Guid(),
                Name = Faker.Random.String(),
                Description = Faker.Random.String(),
                DeliveryPrice = Faker.Random.Decimal()
            };

            var database = CreateMock<IDatabase>();
            database
                .Setup(x => x.FetchAsync<Product>(It.IsNotNull<Sql>()))
                .Returns(Task.FromResult(new List<Product> { product }));

            GetDependency<INPocoDatabaseFactory>().Setup(x => x.CreateDatabase()).Returns(database.Object);

            var products = await TestInstance.ListAsync();
            products.ShouldContain(product);
        }

        [Test]
        public async Task GivenDatabaseError()
        {
            var ex = new Exception(Faker.Random.String());

            var database = CreateMock<IDatabase>();
            database
                .Setup(x => x.FetchAsync<Product>(It.IsNotNull<Sql>()))
                .Throws(ex);

            GetDependency<INPocoDatabaseFactory>().Setup(x => x.CreateDatabase()).Returns(database.Object);

            Should.Throw<DataException>(async () => await TestInstance.ListAsync())
                .InnerException.ShouldBe(ex);
        }
    }
}
