using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Core.Entities;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes CRUD functionality for ProductOption entities.
    /// </summary>
    public interface IProductOptionRepository : IRepository
    {
        IEnumerable<ProductOption> List(Guid productId);
        Task<IEnumerable<ProductOption>> ListAsync(Guid productId);

        ProductOption GetById(Guid productId, Guid id);
        Task<ProductOption> GetByIdAsync(Guid productId, Guid id);
    }
}