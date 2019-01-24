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
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(INPocoDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
        
        protected override string TableName => "Product";

        public IEnumerable<Product> List()
        {
            try
            {
                var sql = new Sql().Append(GenericSelect);

                using (var db = CreateDatabase()) 
                {
                    var products = db.Fetch<Product>(sql);
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new DataException("Select error retrieving all products", e);
            }
        }

        public Product GetById(Guid id)
        {
            try
            {
                var sql = new Sql().Append($"{GenericSelect} where id = @0", id);

                using (var db = CreateDatabase()) 
                {
                    var product = db.Fetch<Product>(sql).SingleOrDefault();
                    return product;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error for {nameof(id)} = {id}", e);
            }
        }

        public IEnumerable<Product> GetByName(string name)
        {
            try
            {
                var sql = new Sql().Append($"{GenericSelect} where lower(name) like @0", $"%{name.ToLower()}%");
                using (var db = CreateDatabase()) 
                {
                    var products = db.Fetch<Product>(sql);
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error for {nameof(name)} = '{name}'", e);
            }
        }
    }
}