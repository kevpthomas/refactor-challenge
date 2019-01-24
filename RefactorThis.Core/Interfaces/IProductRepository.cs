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
        IEnumerable<Product> List();

        Product GetById(Guid id);
        IEnumerable<Product> GetByName(string name);

        Product Add(Product entity);
        Product Update(Product entity);
        void Delete(Guid id);
    }
}