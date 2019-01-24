using System;
using System.Collections.Generic;
using RefactorThis.Core.Entities;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes CRUD functionality for Product entities.
    /// </summary>
    public interface IProductRepository : IRepository
    {
        IEnumerable<Product> List();

        Product GetById(Guid id);
        IEnumerable<Product> GetByName(string name);
    }
}