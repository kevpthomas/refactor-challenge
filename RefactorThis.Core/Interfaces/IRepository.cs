﻿using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes generic CRUD functionality for all entities.
    /// </summary>
    public interface IRepository
    {
        T Add<T>(T entity) where T : Entity;
        void Delete<T>(T entity) where T : Entity;
        T Update<T>(T entity) where T : Entity;
    }
}