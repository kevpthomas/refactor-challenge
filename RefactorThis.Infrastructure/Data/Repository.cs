﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NPoco;
using RefactorThis.Core.Entities;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.SharedKernel;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    public abstract class Repository : IRepository
    {
        private readonly INPocoDatabaseFactory _databaseFactory;

        protected abstract string TableName { get; }

        protected string GenericSelect => $"select * from {TableName}";

        protected Repository(INPocoDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected IDatabase CreateDatabase()
        {
            return _databaseFactory.CreateDatabase();
        }
        
        public T Add<T>(T entity) where T : Entity
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

        public T Update<T>(T entity) where T : Entity
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

        public void Delete<T>(T entity) where T : Entity
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var deleteCount = db.Delete(TableName, nameof(entity.Id), entity);

                    if (deleteCount == 0)
                        throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Delete error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}", e);
            }
        }
    }
}