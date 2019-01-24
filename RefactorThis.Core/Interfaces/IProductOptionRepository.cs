using System;
using System.Collections.Generic;
using RefactorThis.Core.Entities;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes CRUD functionality for ProductOption entities.
    /// </summary>
    public interface IProductOptionRepository : IRepository
    {
        IEnumerable<ProductOption> List(Guid productId);

        ProductOption GetById(Guid productId, Guid id);
    }
}