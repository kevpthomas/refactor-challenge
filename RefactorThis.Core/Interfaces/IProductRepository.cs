using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<IEnumerable<Product>> ListAsync();
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}