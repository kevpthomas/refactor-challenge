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
        Task<IEnumerable<Product>> ListAsync();

        Product GetById(Guid id);
        Task<Product> GetByIdAsync(Guid id);

        IEnumerable<Product> GetByName(string name);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}