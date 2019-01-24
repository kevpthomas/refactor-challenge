using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
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

        public IEnumerable<ProductEntity> List()
        {
            try
            {
                var sql = new Sql().Append(@"select * from product");

                using (var db = CreateDatabase()) 
                {
                    var products = db.Fetch<ProductEntity>(sql);
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new DataException("Select error retrieving all products", e);
            }
        }

        public ProductEntity GetById(Guid id)
        {
            try
            {
                var sql = new Sql().Append(@"select * from product where id = @0", id);

                using (var db = CreateDatabase()) 
                {
                    var product = db.Fetch<ProductEntity>(sql).SingleOrDefault();
                    return product;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error for {nameof(id)} = {id}", e);
            }
        }

        public IEnumerable<ProductEntity> GetByName(string name)
        {
            try
            {
                var sql = new Sql().Append(@"select * from product where lower(name) like @0", $"%{name.ToLower()}%");
                using (var db = CreateDatabase()) 
                {
                    var products = db.Fetch<ProductEntity>(sql);
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Select error for {nameof(name)} = '{name}'", e);
            }
        }

        public ProductEntity Add(ProductEntity entity)
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var foo = db.Insert(entity);
                    return entity;
                    //if (!(db.Insert(entity) is ProductEntity insertedEntity))
                    //    throw new DataException($"Insert error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}");

                    //return insertedEntity;
                }
            }
            catch (DataException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DataException($"Insert error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}", e);
            }
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