using System;
using System.Collections.Generic;
using RefactorThis.Core.Entities;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes CRUD functionality for Product entities.
    /// </summary>
    public interface IProductRepository
    {
        IEnumerable<ProductEntity> List();

        ProductEntity GetById(Guid id);
        IEnumerable<ProductEntity> GetByName(string name);

        ProductEntity Add(ProductEntity entity);
        ProductEntity Update(ProductEntity entity);
        ProductEntity Delete(ProductEntity entity);
    }
}