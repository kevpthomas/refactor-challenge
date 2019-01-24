using System;
using System.Collections.Generic;
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
    public class ListTests : UnitTestBase<ProductRepository>
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

            var database = CreateMock<IDatabase>();
            database
                .Setup(x => x.Fetch<Product>(It.IsNotNull<Sql>()))
                .Returns(new List<Product> { product });

            GetDependency<INPocoDatabaseFactory>().Setup(x => x.CreateDatabase()).Returns(database.Object);

            TestInstance.List().ShouldContain(product);
        }

        [Test]
        public void GivenDatabaseError()
        {
            var ex = new Exception(Faker.Random.String());

            var database = CreateMock<IDatabase>();
            database
                .Setup(x => x.Fetch<Product>(It.IsNotNull<Sql>()))
                .Throws(ex);

            GetDependency<INPocoDatabaseFactory>().Setup(x => x.CreateDatabase()).Returns(database.Object);

            Should.Throw<DataException>(() => TestInstance.List())
                .InnerException.ShouldBe(ex);
        }
    }
}
