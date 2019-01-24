using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    public class ProductOptionRepository : Repository, IProductOptionRepository
    {
        public ProductOptionRepository(INPocoDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
        
        protected override string TableName => "ProductOption";

        public IEnumerable<ProductOption> List(Guid productId)
        {
            try
            {
                var sql = new Sql().Append($"{GenericSelect} where productid = @0", productId);

                using (var db = CreateDatabase()) 
                {
                    var options = db.Fetch<ProductOption>(sql);
                    return options;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error retrieving options for product with {nameof(productId)} = {productId}", e);
            }
        }

        public ProductOption GetById(Guid productId, Guid id)
        {
            try
            {
                var sql = new Sql().Append($"{GenericSelect} where productid = @0 and id = @1", productId, id);

                using (var db = CreateDatabase()) 
                {
                    var product = db.Fetch<ProductOption>(sql).SingleOrDefault();
                    return product;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error for {nameof(id)} = {id}", e);
            }
        }
    }
}