using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using NPoco;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<ProductEntity> List()
        {
            throw new NotImplementedException();
        }

        public ProductEntity GetById(Guid id)
        {
            try
            {
                var sql = new Sql().Append(@"select * from product where id = @0", id);

                using (IDatabase db = new Database("DefaultConnectionString")) 
                {
                    var product = db.Fetch<ProductEntity>(sql).SingleOrDefault();
                    return product;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"SQL error for Id = {id}", e);
            }
        }

        public IEnumerable<ProductEntity> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public ProductEntity Add(ProductEntity entity)
        {
            throw new NotImplementedException();
        }

        public ProductEntity Update(ProductEntity entity)
        {
            throw new NotImplementedException();
        }

        public ProductEntity Delete(ProductEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}