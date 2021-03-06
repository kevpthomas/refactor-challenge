﻿using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json;
using NPoco;
using RefactorThis.Core.Exceptions;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.SharedKernel;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    /// <summary>
    /// Defines shared functionality for all NPoco repositories.
    /// </summary>
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
                    // https://stackoverflow.com/questions/22857315/petapoco-fails-inserting-record-with-guid-newid-primary-key
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
        
        public async Task<T> AddAsync<T>(T entity) where T : Entity
        {
            var id = Guid.Empty;
            
            try
            {
                var block = new ActionBlock<T>(
                    e =>
                    {
                        using (var db = CreateDatabase())
                        {
                            // https://stackoverflow.com/questions/22857315/petapoco-fails-inserting-record-with-guid-newid-primary-key
                            id = (Guid)db.Insert(TableName, nameof(e.Id), false, e);
                        }
                    },
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

                block.Post(entity);
                block.Complete();

                await block.Completion;

                if (id != entity.Id)
                    throw new ArgumentException();

                return entity;

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
                    var updateCount = db.Update(entity);

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

        public async Task<T> UpdateAsync<T>(T entity) where T : Entity
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var updateCount = await db.UpdateAsync(entity);

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
                    var deleteCount = db.Delete(entity);

                    if (deleteCount == 0)
                        throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                throw new DataException($"Delete error for {nameof(entity)} = {JsonConvert.SerializeObject(entity)}", e);
            }
        }

        public async Task DeleteAsync<T>(T entity) where T : Entity
        {
            try
            {
                using (var db = CreateDatabase())
                {
                    var deleteCount = await db.DeleteAsync(entity);

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