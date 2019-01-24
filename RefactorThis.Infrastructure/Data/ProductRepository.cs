using System;
using System.Collections.Generic;
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
        
        protected override string TableName => "Product";

        public IEnumerable<Product> List()
        {
            try
            {
                var sql = new Sql().Append($"select * from {TableName}");

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
                var sql = new Sql().Append($"select * from {TableName} where id = @0", id);

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
                var sql = new Sql().Append($"select * from {TableName} where lower(name) like @0", $"%{name.ToLower()}%");
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

        public Product Add(Product entity)
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var id = (Guid)db.Insert(TableName, nameof(entity.Id), false, entity);

                    if (id != entity.Id)
                        throw new ArgumentException();

                    return entity;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Insert error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}", e);
            }
        }

        public Product Update(Product entity)
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var updateCount = db.Update(TableName, nameof(entity.Id), entity);

                    if (updateCount == 0)
                        throw new ArgumentException();

                    return entity;
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Update error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}", e);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var product = new Product {Id = id};
                    var deleteCount = db.Delete(TableName, nameof(product.Id), product);

                    if (deleteCount == 0)
                        throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Delete error for {nameof(id)} = {JsonConvert.SerializeObject(id)}", e);
            }
        }
    }
}