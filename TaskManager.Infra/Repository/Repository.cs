﻿using Microsoft.EntityFrameworkCore;

using TaskManager.Infra.UnitofWork;

namespace TaskManager.Infra.Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitofWork _unitOfWork;
        public Repository(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>();
        }
        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsEnumerable<T>();
        }
        public T GetOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).FirstOrDefault();
        }
        public void Insert(T entity)
        {
            if (entity != null) _unitOfWork.Context.Set<T>().Add(entity);
        }
        public void Update(object id, T entity)
        {
            if (entity != null)
            {
                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void Delete(object id)
        {
            T entity = _unitOfWork.Context.Set<T>().Find(id);
            Delete(entity);
        }
        public void Delete(T entity)
        {
            if (entity != null) _unitOfWork.Context.Set<T>().Remove(entity);
        }
    }
}
